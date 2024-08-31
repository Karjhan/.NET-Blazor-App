using Application.Requests.Accounts;
using Application.Responses.Accounts;
using Domain.Primitives;

namespace Application.Services;

public interface IAccountService
{
    Task<Result> CreateAdminAsync(CancellationToken cancellationToken = default);

    Task<Result> CreateAccountAsync(CreateAccountRequest request, CancellationToken cancellationToken = default);

    Task<Result<LoginResponse>> LoginAccountAsync(LoginAccountRequest request, CancellationToken cancellationToken = default);

    Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);

    Task<Result> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<GetRoleResponse>>> GetRolesAsync(CancellationToken cancellationToken = default);
    
    // IEnumerable<GetRoleResponse> GetDefaultRoles();

    Task<Result<IEnumerable<GetAccountWithRoleResponse>>> GetAccountsWithRolesAsync(CancellationToken cancellationToken = default);

    Task<Result> ChangeUserRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default);
}