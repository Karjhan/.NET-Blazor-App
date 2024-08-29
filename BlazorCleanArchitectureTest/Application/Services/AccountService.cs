using System.Net.Http.Json;
using Application.Adapters;
using Application.Requests.Accounts;
using Application.Responses.Accounts;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AccountService(
    IBackendApiAdapter backendApiAdapter,
    ILogger<AccountService> logger
) : IAccountService
{
    public async Task<Result> CreateAdminAsync()
    {
        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string createAdminRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiCreateAdminSubPath;
            var response = await publicClient.PostAsync(createAdminRoute, null);
            
            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result!;
        }
        catch (Exception e)
        {
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result> CreateAccountAsync(CreateAccountRequest request)
    {
        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string createAccountRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiCreateAccountSubPath;
            var response = await publicClient.PostAsJsonAsync(createAccountRoute, request);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure(Error.AccountCreationError with { Description = error }); 
            }

            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result!;
        }
        catch (Exception e)
        {
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result<LoginResponse>> LoginAccountAsync(LoginAccountRequest request)
    {
        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string loginRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiLoginAccountSubPath;
            var response = await publicClient.PostAsJsonAsync(loginRoute, request);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<LoginResponse>(Error.AccountLoggingError with { Description = error }); 
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result;
        }
        catch (Exception e)
        {
            return Result.Failure<LoginResponse>(Error.InternalServerError);
        }
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            var refreshTokenRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiRefreshTokenSubPath;
            var response = await publicClient.PostAsJsonAsync(refreshTokenRoute, request);
        
            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<LoginResponse>(new Error("Error.RefreshToken", error)); 
            }
            
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result;
        }
        catch (Exception e)
        {
            return Result.Failure<LoginResponse>(Error.InternalServerError);
        }
    }

    public Task<Result> CreateRoleAsync(CreateRoleRequest request)
    {
        throw new NotImplementedException();
    }

    // public async Task<Result<IEnumerable<GetRoleResponse>>> GetRolesAsync()
    // {
    //     try
    //     {
    //         var privateClient = (await backendApiAdapter.GetPrivateClient()).Value;
    //         const string getRolesRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiGetRolesSubPath;
    //         var response = await privateClient.GetAsync(getRolesRoute);
    //
    //         var error = await CheckResponseStatus(response);
    //         if (!string.IsNullOrEmpty(error))
    //         {
    //             return Result.Failure<IEnumerable<GetRoleResponse>>(Error.RoleNotFound with { Description = error }); 
    //         }
    //
    //         var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetRoleResponse>>();
    //         return result!.ToList();
    //     }
    //     catch (Exception e)
    //     {
    //         return Result.Failure<IEnumerable<GetRoleResponse>>(Error.InternalServerError);
    //     }
    // }
    //
    // public IEnumerable<GetRoleResponse> GetDefaultRoles()
    // {
    //     var result = new List<GetRoleResponse>();
    //     result.Add(new GetRoleResponse("1", RoleConstants.Admin));
    //     result.Add(new GetRoleResponse("2", RoleConstants.User));
    //     return result;
    // }

    public async Task<Result<IEnumerable<GetAccountWithRoleResponse>>> GetAccountsWithRolesAsync()
    {
        try
        {
            var privateClient = (await backendApiAdapter.GetPrivateClient()).Value;
            const string getAccountsWithRolesRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiGetAccountsWithRolesSubPath;
            var response = await privateClient.GetAsync(getAccountsWithRolesRoute);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<IEnumerable<GetAccountWithRoleResponse>>(Error.UserNotFound with { Description = error }); 
            }

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetAccountWithRoleResponse>>();
            return result!.ToList();
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<GetAccountWithRoleResponse>>(Error.InternalServerError);
        }
    }

    public async Task<Result> ChangeUserRoleAsync(UpdateRoleRequest request)
    {
        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string updateRoleRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiUpdateRoleSubPath;
            var response = await publicClient.PostAsJsonAsync(updateRoleRoute, request);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure(new Error("Error.UpdateRole", error)); 
            }

            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result!;
        }
        catch (Exception e)
        {
            return Result.Failure(Error.InternalServerError);
        }
    }

    private static async Task<string> CheckResponseStatus(HttpResponseMessage response)
    {
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseObject = await response.Content.ReadFromJsonAsync<Error>();
                return $"Sorry, unknown error occured.\nError Description: {responseObject!.Description}\nStatus Code: {responseObject.Code}";
            }

            return string.Empty;
        }
        catch (Exception e)
        {
            return $"Sorry, unknown error occured.\nError Description: {Error.InternalServerError.Description}\nStatus Code: {Error.InternalServerError.Code}";
        }
    }
}