using System.Net.Http.Json;
using Application.Adapters;
using Application.Requests.VehicleBrands;
using Application.Requests.VehicleOwners;
using Application.Requests.Vehicles;
using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;
using Application.Responses.Vehicles;
using Application.Utilities;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class VehicleService(
    IBackendApiAdapter backendApiAdapter,
    ILogger<AccountService> logger
) : IVehicleService
{
    public async Task<Result> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("CreateVehicleAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();
            const string createVehicleRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiCreateVehicleSubPath;

            var response = await privateClient.PostAsJsonAsync(createVehicleRoute, request, cancellationToken);
            
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to create vehicle. {Error}", error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("CreateVehicleAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating the vehicle.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result<IEnumerable<GetVehicleResponse>>> GetVehiclesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetVehiclesAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();;
            const string getVehiclesRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiGetVehiclesSubPath;
            
            logger.LogDebug("Sending GET request to {Route} to get available vehicles.", getVehiclesRoute);
            
            var response = await privateClient.GetAsync(getVehiclesRoute, cancellationToken);
    
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get available vehicles. {Error}", error);
                return Result.Failure<IEnumerable<GetVehicleResponse>>(Error.VehicleNotFound with { Description = error }); 
            }
    
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetVehicleResponse>>(cancellationToken);
            logger.LogInformation("GetVehiclesAsync completed successfully.");
            return result!.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting available vehicles.");
            return Result.Failure<IEnumerable<GetVehicleResponse>>(Error.InternalServerError);
        }
    }

    public async Task<Result<GetVehicleResponse>> GetVehicleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetVehicleAsync started for {VehicleId}.", id);

        try
        {
            var privateClient = await GetPrivateClient();
            string getVehicleRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiGetSingleVehicleSubPath(id.ToString());

            logger.LogDebug("Sending GET request to {Route} to get vehicle data for {VehicleId}.", getVehicleRoute, id);

            var response = await privateClient.GetAsync(getVehicleRoute, cancellationToken);

            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get vehicle for {VehicleId}. {Error}", id, error);
                return Result.Failure<GetVehicleResponse>(Error.VehicleNotFound with { Description = error });
            }
            
            var result = await response.Content.ReadFromJsonAsync<GetVehicleResponse>(cancellationToken);
            logger.LogInformation("GetVehicleAsync completed successfully for {VehicleId}.", id);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting vehicle for {VehicleId}.", id);
            return Result.Failure<GetVehicleResponse>(Error.InternalServerError);
        }
    }

    public async Task<Result> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("DeleteVehicleAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();
            string deleteVehicleRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiDeleteVehicleSubPath(id.ToString());

            var response = await privateClient.DeleteAsync(deleteVehicleRoute, cancellationToken);
            
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to delete vehicle with {Id}. {Error}", id, error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("DeleteVehicleAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while deleting the vehicle.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result> UpdateVehicleAsync(UpdateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("UpdateVehicleAsync started for {VehicleId}.", request.Id);

        try
        {
            var privateClient = await GetPrivateClient();
            const string updateVehicleRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiUpdateVehicleSubPath;

            logger.LogDebug("Sending PUT request to {Route} to change vehicle data for {VehicleId}.", updateVehicleRoute, request.Id);

            var response = await privateClient.PutAsJsonAsync(updateVehicleRoute, request, cancellationToken);

            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to update vehicle for {VehicleId}. {Error}", request.Id, error);
                return Result.Failure(new Error("Error.UpdateVehicle", error));
            }

            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("UpdateVehicleAsync completed successfully for {VehicleId}.", request.Id);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while updating vehicle for {VehicleId}.", request.Id);
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result> CreateVehicleBrandAsync(CreateVehicleBrandRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("CreateVehicleBrandAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();
            const string createVehicleBrandRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiCreateVehicleBrandSubPath;

            var response = await privateClient.PostAsJsonAsync(createVehicleBrandRoute, request, cancellationToken);
            
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to create vehicle brand. {Error}", error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("CreateVehicleBrandAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating the vehicle brand.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result<IEnumerable<GetVehicleBrandResponse>>> GetVehicleBrandsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetVehicleBrandsAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();;
            const string getVehicleBrandsRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiGetVehicleBrandsSubPath;
            
            logger.LogDebug("Sending GET request to {Route} to get available vehicle brands.", getVehicleBrandsRoute);
            
            var response = await privateClient.GetAsync(getVehicleBrandsRoute, cancellationToken);
    
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get available vehicle brands. {Error}", error);
                return Result.Failure<IEnumerable<GetVehicleBrandResponse>>(Error.VehicleBrandNotFound with { Description = error }); 
            }
    
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetVehicleBrandResponse>>(cancellationToken);
            logger.LogInformation("GetVehicleBrandsAsync completed successfully.");
            return result!.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting available vehicle brands.");
            return Result.Failure<IEnumerable<GetVehicleBrandResponse>>(Error.InternalServerError);
        }
    }

    public async Task<Result<GetVehicleBrandResponse>> GetVehicleBrandAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetVehicleBrandAsync started for {VehicleId}.", id);

        try
        {
            var privateClient = await GetPrivateClient();
            string getVehicleBrandRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiGetSingleVehicleBrandSubPath(id.ToString());

            logger.LogDebug("Sending GET request to {Route} to get vehicle brand data for {VehicleBrandId}.", getVehicleBrandRoute, id);

            var response = await privateClient.GetAsync(getVehicleBrandRoute, cancellationToken);

            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get vehicle brand for {VehicleBrandId}. {Error}", id, error);
                return Result.Failure<GetVehicleBrandResponse>(Error.VehicleBrandNotFound with { Description = error });
            }
            
            var result = await response.Content.ReadFromJsonAsync<GetVehicleBrandResponse>(cancellationToken);
            logger.LogInformation("GetVehicleBrandAsync completed successfully for {VehicleBrandId}.", id);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting vehicle brand for {VehicleBrandId}.", id);
            return Result.Failure<GetVehicleBrandResponse>(Error.InternalServerError);
        }
    }

    public async Task<Result> DeleteVehicleBrandAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("DeleteVehicleBrandAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();
            string deleteVehicleBrandRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiDeleteVehicleBrandSubPath(id.ToString());

            var response = await privateClient.DeleteAsync(deleteVehicleBrandRoute, cancellationToken);
            
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to delete vehicle brand with {Id}. {Error}", id, error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("DeleteVehicleBrandAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while deleting the vehicle brand.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result> UpdateVehicleBrandAsync(UpdateVehicleBrandRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("UpdateVehicleBrandAsync started for {VehicleBrandId}.", request.Id);

        try
        {
            var privateClient = await GetPrivateClient();
            const string updateVehicleBrandRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiUpdateVehicleBrandSubPath;

            logger.LogDebug("Sending PUT request to {Route} to change vehicle data for {VehicleBrandId}.", updateVehicleBrandRoute, request.Id);

            var response = await privateClient.PutAsJsonAsync(updateVehicleBrandRoute, request, cancellationToken);

            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to update vehicle brand for {VehicleBrandId}. {Error}", request.Id, error);
                return Result.Failure(new Error("Error.UpdateVehicleBrand", error));
            }

            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("UpdateVehicleBrandAsync completed successfully for {VehicleId}.", request.Id);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while updating vehicle brand for {VehicleBrandId}.", request.Id);
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result> CreateVehicleOwnerAsync(CreateVehicleOwnerRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("CreateVehicleOwnerAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();
            const string createVehicleOwnerRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiCreateVehicleOwnerSubPath;

            var response = await privateClient.PostAsJsonAsync(createVehicleOwnerRoute, request, cancellationToken);
            
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to create vehicle owner. {Error}", error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("CreateVehicleOwnerAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while creating the vehicle owner.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result<IEnumerable<GetVehicleOwnerResponse>>> GetVehicleOwnersAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetVehicleOwnersAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();;
            const string getVehicleOwnersRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiGetVehicleOwnersSubPath;
            
            logger.LogDebug("Sending GET request to {Route} to get available vehicle owners.", getVehicleOwnersRoute);
            
            var response = await privateClient.GetAsync(getVehicleOwnersRoute, cancellationToken);
    
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get available vehicle owners. {Error}", error);
                return Result.Failure<IEnumerable<GetVehicleOwnerResponse>>(Error.VehicleOwnerNotFound with { Description = error }); 
            }
    
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetVehicleOwnerResponse>>(cancellationToken);
            logger.LogInformation("GetVehicleOwnersAsync completed successfully.");
            return result!.ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting available vehicle owners.");
            return Result.Failure<IEnumerable<GetVehicleOwnerResponse>>(Error.InternalServerError);
        }
    }

    public async Task<Result<GetVehicleOwnerResponse>> GetVehicleOwnerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("GetVehicleOwnerAsync started for {VehicleOwnerId}.", id);

        try
        {
            var privateClient = await GetPrivateClient();
            string getVehicleOwnerRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiGetSingleVehicleOwnerSubPath(id.ToString());

            logger.LogDebug("Sending GET request to {Route} to get vehicle owner data for {VehicleOwnerId}.", getVehicleOwnerRoute, id);

            var response = await privateClient.GetAsync(getVehicleOwnerRoute, cancellationToken);

            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to get vehicle owner for {VehicleOwnerId}. {Error}", id, error);
                return Result.Failure<GetVehicleOwnerResponse>(Error.VehicleNotFound with { Description = error });
            }
            
            var result = await response.Content.ReadFromJsonAsync<GetVehicleOwnerResponse>(cancellationToken);
            logger.LogInformation("GetVehicleOwnerAsync completed successfully for {VehicleOwnerId}.", id);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting vehicle owner for {VehicleId}.", id);
            return Result.Failure<GetVehicleOwnerResponse>(Error.InternalServerError);
        }
    }

    public async Task<Result> DeleteVehicleOwnerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("DeleteVehicleOwnerAsync started.");
        
        try
        {
            var privateClient = await GetPrivateClient();
            string deleteVehicleOwnerRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiDeleteVehicleOwnerSubPath(id.ToString());

            var response = await privateClient.DeleteAsync(deleteVehicleOwnerRoute, cancellationToken);
            
            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to delete vehicle owner with {Id}. {Error}", id, error);
                return Result.Failure(Error.InternalServerError with { Description = error }); 
            }
            
            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("DeleteVehicleOwnerAsync completed successfully.");
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while deleting the vehicle owner.");
            return Result.Failure(Error.InternalServerError);
        }
    }

    public async Task<Result> UpdateVehicleOwnerAsync(UpdateVehicleOwnerRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("UpdateVehicleOwnerAsync started for {VehicleOwnerId}.", request.Id);

        try
        {
            var privateClient = await GetPrivateClient();
            const string updateVehicleOwnerRoute = ApplicationConstants.ApiVehicleBasePath + ApplicationConstants.ApiUpdateVehicleOwnerSubPath;

            logger.LogDebug("Sending PUT request to {Route} to change vehicle data for {VehicleOwnerId}.", updateVehicleOwnerRoute, request.Id);

            var response = await privateClient.PutAsJsonAsync(updateVehicleOwnerRoute, request, cancellationToken);

            var error = await CustomHttpHandler.CheckResponseStatus(response);
            if (!string.IsNullOrEmpty(error))
            {
                logger.LogWarning("Failed to update vehicle owner for {VehicleOwnerId}. {Error}", request.Id, error);
                return Result.Failure(new Error("Error.UpdateVehicleOwner", error));
            }

            var stringResult = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var result = string.IsNullOrEmpty(stringResult) ? Result.Success() : await response.Content.ReadFromJsonAsync<Result>(cancellationToken);
            logger.LogInformation("UpdateVehicleOwnerAsync completed successfully for {VehicleOwnerId}.", request.Id);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while updating vehicle owner for {VehicleOwnerId}.", request.Id);
            return Result.Failure(Error.InternalServerError);
        }
    }

    private async Task<HttpClient> GetPrivateClient()
    {
        return (await backendApiAdapter.GetPrivateClient()).Value;
    }
}