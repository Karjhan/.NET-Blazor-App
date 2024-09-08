using Domain.Primitives;

namespace Application.Services;

public interface IExternalAuthService
{
    Task<Result> SetAuthTokenAsync(string authorizationCode, CancellationToken cancellationToken = default);

    Task<Result> ExchangeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}