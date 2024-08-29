using System.Security.Claims;
using Application.Adapters;
using Application.Responses.Credentials;
using Microsoft.AspNetCore.Components.Authorization;

namespace Application.Utilities;

public class CustomAuthenticationStateProvider(
    ILocalStorageAdapter localStorageAdapter
) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var localStorageTokens = (await localStorageAdapter.GetModelFromToken()).Value;
        if (string.IsNullOrEmpty(localStorageTokens.AccessToken))
        {
            return await Task.FromResult(new AuthenticationState(GetAnonymous()));
        }

        var getUserClaims = IdentityUtilities.DecryptToken(localStorageTokens.AccessToken);
        if (getUserClaims is null)
        {
            return await Task.FromResult(new AuthenticationState(GetAnonymous()));
        }

        var claimsPrincipal = IdentityUtilities.SetClaimPrincipal(getUserClaims);
        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public async Task UpdateAuthenticationState(LocalStorageDTO localStorageDto)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        var isAnyToken = localStorageDto.AccessToken != null || localStorageDto.RefreshToken != null;
        if (isAnyToken)
        {
            await localStorageAdapter.SetBrowserLocalStorage(localStorageDto);
            var getUserClaims = IdentityUtilities.DecryptToken(localStorageDto.AccessToken);
            claimsPrincipal = IdentityUtilities.SetClaimPrincipal(getUserClaims);
        }
        else
        {
            await localStorageAdapter.RemoveTokenFromBrowserLocalStorage();
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private ClaimsPrincipal GetAnonymous()
    {
        return new(new ClaimsIdentity());
    }
}