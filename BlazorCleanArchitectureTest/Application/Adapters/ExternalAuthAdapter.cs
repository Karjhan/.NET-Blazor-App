using Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Application.Adapters;

public class ExternalAuthAdapter(
    IHttpClientFactory httpClientFactory,
    ILogger<ExternalAuthAdapter> logger
) : IExternalAuthAdapter
{
    public HttpClient GetAuthClient()
    {
        logger.LogInformation("Creating HttpClient for {ClientName}.", ApplicationConstants.ExternalAuthClientName);
        return httpClientFactory.CreateClient(ApplicationConstants.ExternalAuthClientName);
    }
}