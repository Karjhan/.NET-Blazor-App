using System.Net.Http.Json;
using Application.Adapters;
using Application.Responses.Credentials;
using Application.Utilities;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ExternalAuthService(
    IConfiguration configuration,
    ILocalStorageAdapter localStorageAdapter,
    IBackendApiAdapter backendApiAdapter,
    IExternalAuthAdapter externalAuthAdapter,
    ILogger<ExternalAuthService> logger
) : IExternalAuthService
{
    public async Task<Result> SetAuthTokenAsync(string authorizationCode, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting the process to exchange auth code {AuthCode} for token.", authorizationCode);

        try
        {
            var externalAuthClient = externalAuthAdapter.GetAuthClient();
            var publicApiClient = backendApiAdapter.GetPublicClient();
            const string exchangeExternalTokenRoute = ApplicationConstants.ApiExternalAuthPath + ApplicationConstants.ApiExchangeExternalTokenSubPath;

            var tokenRequest = new Dictionary<string, string>()
            {
                { "client_id", configuration.GetSection("ThirdPartyClientId").Value! },
                { "client_secret", configuration.GetSection("ThirdPartyClientSecret").Value! },
                { "code", authorizationCode },
                { "grant_type", "authorization_code" },
                {
                    "redirect_uri",
                    $"{configuration.GetSection("BaseAddress").Value!}{ApplicationConstants.ExternalAuthRedirectRoute}"
                },
                { "code_verifier", (await localStorageAdapter.GetCodeVerifier()).Value },
                { "code_challenge_method", "S256" }
            };
            logger.LogDebug("Token request constructed for authorization code {AuthCode}. Request: {TokenRequest}",
                authorizationCode, tokenRequest);

            var response = await externalAuthClient.PostAsync(
                $"{configuration.GetSection("DefaultThirdPartyUrl").Value}connect/token",
                new FormUrlEncodedContent(tokenRequest),
                cancellationToken
            );

            logger.LogInformation("Sent request to external auth service for authorization code {AuthCode}.",
                authorizationCode);

            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to retrieve token for authorization code {AuthCode}. Error: {Error}",
                    authorizationCode, error);
                return Result.Failure(new Error("Error.ExternalAuth", error));
            }

            var result = await response.Content.ReadFromJsonAsync<ExternalAuthAccessDTO>(cancellationToken);
            logger.LogInformation("Received authorization token for code {AuthCode}.", authorizationCode);
            
            logger.LogDebug("Sending GET request to {Route}", exchangeExternalTokenRoute);
            var newAccessTokenResponse = await publicApiClient.GetAsync($"{exchangeExternalTokenRoute}/{result!.AccessToken}", cancellationToken);
            
            error = await CustomHttpHandler.CheckResponseStatus(newAccessTokenResponse);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to exchange external token. {Error}", error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var newAccessToken = (await newAccessTokenResponse.Content.ReadAsStringAsync(cancellationToken)).Trim('"');

            await localStorageAdapter.SetBrowserLocalStorage(new LocalStorageDTO() { AccessToken = newAccessToken, RefreshToken = AccountUtilities.GenerateRefreshToken()});
            logger.LogInformation("Stored local storage model in browser local storage.");

            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while exchanging auth code {AuthCode} for authorization token.",
                authorizationCode);
            return Result.Failure(Error.InternalServerError);
        }
        finally
        {
            logger.LogInformation("Attempting to remove code verifier from local storage.");
            await localStorageAdapter.RemoveCodeVerifierFromBrowserLocalStorage();
        }
    }

    public async Task<Result> ExchangeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return Result.Success();
    }
}