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
    public async Task<Result> CreateAdminAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("CreateAdminAsync started.");
        
        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string createAdminRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiCreateAdminSubPath;
            
            logger.LogDebug("Sending POST request to {Route}", createAdminRoute);
            var response = await publicClient.PostAsync(createAdminRoute, null, cancellationToken);
            
            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to create admin. {Error}", error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("CreateAdminAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating an admin.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result> CreateAccountAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("CreateAccountAsync started for {UserName}.", request.Name);
        
        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string createAccountRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiCreateAccountSubPath;
            
            logger.LogDebug("Sending POST request to {Route} for {UserName}.", createAccountRoute, request.Name);
            var response = await publicClient.PostAsJsonAsync(createAccountRoute, request, cancellationToken);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to create account for {UserName}. {Error}", request.Name, error);
                return Result.Failure(Error.AccountCreationError with { Description = error }); 
            }

            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("CreateAccountAsync completed successfully for {UserName}.", request.Name);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating an account for {UserName}.", request.Name);
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result<LoginResponse>> LoginAccountAsync(LoginAccountRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("LoginAccountAsync started for {UserName}.", request.EmailAddress);

        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string loginRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiLoginAccountSubPath;

            logger.LogDebug("Sending POST request to {Route} for {UserName}.", loginRoute, request.EmailAddress);

            var response = await publicClient.PostAsJsonAsync(loginRoute, request, cancellationToken);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to log in account for {UserName}. {Error}", request.EmailAddress, error);
                return Result.Failure<LoginResponse>(Error.AccountLoggingError with { Description = error });
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
            logger.LogInformation("LoginAccountAsync completed successfully for {UserName}.", request.EmailAddress);
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while logging in for {UserName}.", request.EmailAddress);
            return Result.Failure<LoginResponse>(Error.InternalServerError);
        }
    }

    public async Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("RefreshTokenAsync started.");

        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            var refreshTokenRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiRefreshTokenSubPath;

            logger.LogDebug("Sending POST request to {Route} to refresh token.", refreshTokenRoute);

            var response = await publicClient.PostAsJsonAsync(refreshTokenRoute, request, cancellationToken);
        
            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to refresh token. {Error}", error);
                return Result.Failure<LoginResponse>(new Error("Error.RefreshToken", error));
            }
            
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
            logger.LogInformation("RefreshTokenAsync completed successfully.");
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while refreshing token.");
            return Result.Failure<LoginResponse>(Error.InternalServerError);
        }
    }

    public async Task<Result> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("CreateRoleAsync started.");

        try
        {
            var privateClient = (await backendApiAdapter.GetPrivateClient()).Value;
            const string createRoleRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiCreateRoleSubPath;

            logger.LogDebug("Sending POST request to {Route} to create role {NewRole}.", createRoleRoute, request.Name);

            var response = await privateClient.PostAsJsonAsync(createRoleRoute, request, cancellationToken);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to create new role. {Error}", error);
                return Result.Failure(new Error("Error.CreateRole", error));
            }

            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("CreateRoleAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating new role.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result<IEnumerable<GetRoleResponse>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetRolesAsync started.");
        
        try
        {
            var privateClient = (await backendApiAdapter.GetPrivateClient()).Value;
            const string getRolesRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiGetRolesSubPath;
            
            logger.LogDebug("Sending GET request to {Route} to get available roles.", getRolesRoute);
            
            var response = await privateClient.GetAsync(getRolesRoute, cancellationToken);
    
            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get available roles. {Error}", error);
                return Result.Failure<IEnumerable<GetRoleResponse>>(Error.RoleNotFound with { Description = error }); 
            }
    
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetRoleResponse>>(cancellationToken);
            logger.LogInformation("GetRolesAsync completed successfully.");
            return result!.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting available roles.");
            return Result.Failure<IEnumerable<GetRoleResponse>>(Error.InternalServerError);
        }
    }
    
    // public IEnumerable<GetRoleResponse> GetDefaultRoles()
    // {
    //     var result = new List<GetRoleResponse>();
    //     result.Add(new GetRoleResponse("1", RoleConstants.Admin));
    //     result.Add(new GetRoleResponse("2", RoleConstants.User));
    //     return result;
    // }

    public async Task<Result<IEnumerable<GetAccountWithRoleResponse>>> GetAccountsWithRolesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetAccountsWithRolesAsync started.");

        try
        {
            var privateClient = (await backendApiAdapter.GetPrivateClient()).Value;
            const string getAccountsWithRolesRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiGetAccountsWithRolesSubPath;

            logger.LogDebug("Sending GET request to {Route} to get accounts with roles.", getAccountsWithRolesRoute);

            var response = await privateClient.GetAsync(getAccountsWithRolesRoute, cancellationToken);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get accounts with roles. {Error}", error);
                return Result.Failure<IEnumerable<GetAccountWithRoleResponse>>(Error.UserNotFound with { Description = error });
            }

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetAccountWithRoleResponse>>(cancellationToken);
            logger.LogInformation("GetAccountsWithRolesAsync completed successfully.");
            return result!.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting accounts with roles.");
            return Result.Failure<IEnumerable<GetAccountWithRoleResponse>>(Error.InternalServerError);
        }
    }

    public async Task<Result> ChangeUserRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("ChangeUserRoleAsync started for {UserId} to {NewRole}.", request.UserEmail, request.RoleName);

        try
        {
            var publicClient = backendApiAdapter.GetPublicClient();
            const string updateRoleRoute = ApplicationConstants.ApiAccountBasePath + ApplicationConstants.ApiUpdateRoleSubPath;

            logger.LogDebug("Sending POST request to {Route} to change user role for {UserId}.", updateRoleRoute, request.UserEmail);

            var response = await publicClient.PostAsJsonAsync(updateRoleRoute, request, cancellationToken);

            var error = await CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                // logger.LogWarning("Failed to change role for {UserId}. {Error}", request.UserEmail, error);
                return Result.Failure(new Error("Error.UpdateRole", error));
            }

            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("ChangeUserRoleAsync completed successfully for {UserId}.", request.UserEmail);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while changing user role for {UserId}.", request.UserEmail);
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