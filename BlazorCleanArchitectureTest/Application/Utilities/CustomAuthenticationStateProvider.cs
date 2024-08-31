using System.Security.Claims;
using Application.Adapters;
using Application.Responses.Credentials;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace Application.Utilities;

public class CustomAuthenticationStateProvider(
    ILocalStorageAdapter localStorageAdapter,
    ILogger<CustomAuthenticationStateProvider> logger
) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        logger.LogInformation("GetAuthenticationStateAsync started.");

        var localStorageTokens = (await localStorageAdapter.GetModelFromToken()).Value;

        if (string.IsNullOrEmpty(localStorageTokens.AccessToken))
        {
            logger.LogWarning("No access token found in local storage. Returning anonymous user.");
            return await Task.FromResult(new AuthenticationState(GetAnonymous()));
        }

        logger.LogDebug("Access token found, attempting to decrypt token.");

        var userClaims = IdentityUtilities.DecryptToken(localStorageTokens.AccessToken);
        if (userClaims is null)
        {
            logger.LogWarning("Failed to decrypt access token. Returning anonymous user.");
            return await Task.FromResult(new AuthenticationState(GetAnonymous()));
        }

        logger.LogDebug("Token successfully decrypted. Setting claims principal.");
        var claimsPrincipal = IdentityUtilities.SetClaimPrincipal(userClaims);
        logger.LogInformation("GetAuthenticationStateAsync completed successfully.");
        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public async Task UpdateAuthenticationState(LocalStorageDTO localStorageDto)
    {
        logger.LogInformation("UpdateAuthenticationState started.");

        var claimsPrincipal = new ClaimsPrincipal();
        var isAnyToken = localStorageDto.AccessToken != null || localStorageDto.RefreshToken != null;

        if (isAnyToken)
        {
            logger.LogDebug("Tokens found. Updating local storage and setting claims principal.");

            await localStorageAdapter.SetBrowserLocalStorage(localStorageDto);
            var userClaims = IdentityUtilities.DecryptToken(localStorageDto.AccessToken);
            claimsPrincipal = IdentityUtilities.SetClaimPrincipal(userClaims);
        }
        else
        {
            logger.LogWarning("No tokens found. Removing tokens from local storage and setting anonymous user.");
            await localStorageAdapter.RemoveTokenFromBrowserLocalStorage();
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        logger.LogInformation("UpdateAuthenticationState completed.");
    }

    private ClaimsPrincipal GetAnonymous()
    {
        return new(new ClaimsIdentity());
    }
}