using System.Net;
using System.Net.Http.Headers;
using Application.Adapters;
using Application.Requests.Accounts;
using Application.Responses.Credentials;
using Application.Services;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Components;

namespace Application.Utilities;

public class CustomHttpHandler(
    LocalStorageAdapter localStorageAdapter,
    NavigationManager navigationManager,
    IAccountService accountService
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
                return await base.SendAsync(request, cancellationToken);
            }
            
            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                var tokenModel = (await localStorageAdapter.GetModelFromToken()).Value;
                if (tokenModel is null)
                {
                    return result;
                }

                var newJwtToken = await GetRefreshToken(tokenModel.RefreshToken);
                if (string.IsNullOrEmpty(newJwtToken))
                {
                    return result;
                }
                
                request.Headers.Authorization = new AuthenticationHeaderValue(LocalStorageConstants.HttpClientHeaderScheme, newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }

            return result;
        }
        catch (Exception e)
        {
            return null!;
        }
    }

    private async Task<string> GetRefreshToken(string refreshToken)
    {
        try
        {
            var response = (await accountService.RefreshTokenAsync(new RefreshTokenRequest() { Token = refreshToken })).Value;
            var isResponseTokenNull = response is null || response.Token is null;
            if (isResponseTokenNull)
            {
                await localStorageAdapter.RemoveTokenFromBrowserLocalStorage();
                NavigateToLogin();
                return null!;
            }

            await localStorageAdapter.RemoveTokenFromBrowserLocalStorage();
            await localStorageAdapter.SetBrowserLocalStorage(new LocalStorageDTO(){RefreshToken = response!.RefreshToken, AccessToken = response.Token});

            return response.Token!;
        }
        catch (Exception e)
        {
            return null!;
        }    
    }

    private void NavigateToLogin()
    {
        navigationManager.NavigateTo(navigationManager.BaseUri, true, true);
    }
}