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
        logger.LogInformation("Creating public HttpClient.");
        return CreateClient();
    }

    public async Task<Result<HttpClient>> GetPrivateClient()
    {
        logger.LogInformation("Attempting to create private HttpClient with authorization.");
        
        try
        {
            var client = CreateClient();
            var result = await localStorageAdapter.GetModelFromToken();

            if (result.IsFailure)
            {
                logger.LogWarning("Failed to retrieve token from local storage.");
                return client;
            }

            var localStorageDTO = result.Value;
            
            if (string.IsNullOrEmpty(localStorageDTO.AccessToken))
            {
                logger.LogWarning("Access token is null or empty, returning client without authorization.");
                return client;
            }
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(LocalStorageConstants.HttpClientHeaderScheme, localStorageDTO.AccessToken);
            
            logger.LogInformation("Successfully added authorization header to HttpClient.");
            return client;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating private HttpClient.");
            return httpClientFactory.CreateClient();
        }
    }
    
    private HttpClient CreateClient()
    {
        logger.LogInformation("Creating HttpClient for {ClientName}.", ApplicationConstants.BackendApiClientName);
        return httpClientFactory.CreateClient(ApplicationConstants.BackendApiClientName);
    }
}