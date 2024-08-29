using Application.Requests.Accounts;
using Application.Responses.Accounts;
using Domain.Primitives;

namespace Application.Services;

public interface IAccountService
{
    Task<Result> CreateAdminAsync();

    Task<Result> CreateAccountAsync(CreateAccountRequest request);

    Task<Result<LoginResponse>> LoginAccountAsync(LoginAccountRequest request);

    Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);

    Task<Result> CreateRoleAsync(CreateRoleRequest request);

    // Task<Result<IEnumerable<GetRoleResponse>>> GetRolesAsync();
    //
    // IEnumerable<GetRoleResponse> GetDefaultRoles();

    Task<Result<IEnumerable<GetAccountWithRoleResponse>>> GetAccountsWithRolesAsync();

    Task<Result> ChangeUserRoleAsync(UpdateRoleRequest request);
}