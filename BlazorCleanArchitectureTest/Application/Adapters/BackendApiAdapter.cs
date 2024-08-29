using System.Net.Http.Headers;
using Domain.Primitives;
using Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Application.Adapters;

public class BackendApiAdapter(
    IHttpClientFactory httpClientFactory,
    ILocalStorageAdapter localStorageAdapter,
    ILogger<BackendApiAdapter> logger
) : IBackendApiAdapter
{
    public HttpClient GetPublicClient()
    {
        return CreateClient();
    }

    public async Task<Result<HttpClient>> GetPrivateClient()
    {
        try
        {
            var client = CreateClient();
            var localStorageDTO = (await localStorageAdapter.GetModelFromToken()).Value;
            if (string.IsNullOrEmpty(localStorageDTO.AccessToken))
            {
                return client;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(LocalStorageConstants.HttpClientHeaderScheme, localStorageDTO.AccessToken);
            return client;
        }
        catch (Exception e)
        {
            return httpClientFactory.CreateClient();
        }
    }
    
    private HttpClient CreateClient()
    {
        return httpClientFactory.CreateClient(ApplicationConstants.BackendApiClientName);
    }
}