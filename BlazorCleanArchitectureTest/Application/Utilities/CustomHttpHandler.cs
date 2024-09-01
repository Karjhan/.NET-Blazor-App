using System.Net;
using System.Net.Http.Headers;
using Application.Adapters;
using Application.Requests.Accounts;
using Application.Responses.Credentials;
using Application.Services;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Application.Utilities;

public class CustomHttpHandler(
    ILocalStorageAdapter localStorageAdapter,
    NavigationManager navigationManager,
    IAccountService accountService,
    ILogger<CustomHttpHandler> logger
) : DelegatingHandler
{
    protected async override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains(ApplicationConstants.ApiAccountBasePath+ApplicationConstants.ApiLoginAccountSubPath);
            bool registerUrl = request.RequestUri.AbsoluteUri.Contains(ApplicationConstants.ApiAccountBasePath+ApplicationConstants.ApiCreateAccountSubPath);
            bool refreshTokenUrl = request.RequestUri.AbsoluteUri.Contains(ApplicationConstants.ApiAccountBasePath+ApplicationConstants.ApiRefreshTokenSubPath);
            bool adminCreateUrl = request.RequestUri.AbsoluteUri.Contains(ApplicationConstants.ApiAccountBasePath+ApplicationConstants.ApiCreateAdminSubPath);

            if (loginUrl || registerUrl || refreshTokenUrl || adminCreateUrl)
            {
                logger.LogInformation("Request to login, register, refresh token, or create admin. Sending request without further processing.");
                return await base.SendAsync(request, cancellationToken);
            }
            
            logger.LogInformation("Processing request to {RequestUri}", request.RequestUri);
            
            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                logger.LogWarning("Received Unauthorized response for request to {RequestUri}. Attempting to refresh token.", request.RequestUri);
                
                var tokenModel = (await localStorageAdapter.GetModelFromToken()).Value;
                if (tokenModel is null)
                {
                    logger.LogWarning("Token model is null. Returning original Unauthorized response.");
                    return result;
                }

                var newJwtToken = await GetRefreshToken(tokenModel.RefreshToken);
                if (string.IsNullOrEmpty(newJwtToken))
                {
                    logger.LogWarning("Failed to refresh token. Returning original Unauthorized response.");
                    return result;
                }
                
                logger.LogInformation("Successfully refreshed token. Retrying request with new token.");
                
                request.Headers.Authorization = new AuthenticationHeaderValue(LocalStorageConstants.HttpClientHeaderScheme, newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }

            logger.LogInformation("Request to {RequestUri} completed successfully with status code {StatusCode}", request.RequestUri, result.StatusCode);
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while processing request to {RequestUri}", request.RequestUri);
            return null!;
        }
    }

    private async Task<string> GetRefreshToken(string refreshToken)
    {
        try
        {
            logger.LogInformation("Attempting to refresh token using refresh token {RefreshToken}", refreshToken);
            
            var response = (await accountService.RefreshTokenAsync(new RefreshTokenRequest() { Token = refreshToken })).Value;
            var isResponseTokenNull = response is null || response.Token is null;
            if (isResponseTokenNull)
            {
                logger.LogWarning("Refresh token response is null. Removing token from local storage and navigating to login.");
                
                await localStorageAdapter.RemoveTokenFromBrowserLocalStorage();
                NavigateToLogin();
                return null!;
            }
            
            logger.LogInformation("Refresh token succeeded. Updating local storage with new tokens.");

            await localStorageAdapter.RemoveTokenFromBrowserLocalStorage();
            await localStorageAdapter.SetBrowserLocalStorage(new LocalStorageDTO(){RefreshToken = response!.RefreshToken, AccessToken = response.Token});

            return response.Token!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while refreshing token {RefreshToken}", refreshToken);
            return null!;
        }    
    }

    private void NavigateToLogin()
    {
        logger.LogInformation("Navigating to login page.");
        navigationManager.NavigateTo(navigationManager.BaseUri, true, true);
    }
}