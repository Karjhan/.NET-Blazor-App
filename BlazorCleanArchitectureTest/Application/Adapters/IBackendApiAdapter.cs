using Domain.Primitives;

namespace Application.Adapters;

public interface IBackendApiAdapter
{
    HttpClient GetPublicClient();

    Task<Result<HttpClient>> GetPrivateClient();
}