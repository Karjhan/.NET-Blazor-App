using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;
using Application.Responses.Vehicles;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Queries.GetSingleVehicle;

public class GetSingleVehicleQueryHandler(
    ApplicationContext context,
    ILogger<GetSingleVehicleQueryHandler> logger
) : IQueryHandler<GetSingleVehicleQuery, GetVehicleResponse>
{
    public async Task<Result<GetVehicleResponse>> Handle(GetSingleVehicleQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetSingleVehicleQuery for Vehicle Id: {VehicleId}", request.Id);
        
        var vehicle = await context.Vehicles.FirstOrDefaultAsync(
            vehicle => vehicle.Id == request.Id,
            cancellationToken
        );
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle with Id: {VehicleId} not found.", request.Id);
            return Result.Failure<GetVehicleResponse>(Error.VehicleNotFound);
        }
        
        logger.LogInformation("Vehicle with Id: {VehicleId} found. Preparing response.", request.Id);
        
        var existingVehicleOwner = await context.VehicleOwners.FirstOrDefaultAsync(
            owner => owner.Id == vehicle.VehicleOwnerId,
            cancellationToken
        );
        if (existingVehicleOwner is null)
        {
            logger.LogWarning("VehicleOwner with Id: {VehicleOwnerId} not found.", vehicle.VehicleOwnerId);
            return Result.Failure<GetVehicleResponse>(Error.VehicleOwnerNotFound);
        }
        
        var existingVehicleBrand = await context.VehicleBrands.FirstOrDefaultAsync(
            brand => brand.Id == vehicle.VehicleBrandId,
            cancellationToken
        );
        if (existingVehicleBrand is null)
        {
            logger.LogWarning("VehicleBrand with Id: {VehicleBrandId} not found.", vehicle.VehicleBrandId);
            return Result.Failure<GetVehicleResponse>(Error.VehicleBrandNotFound);
        }

        var result = new GetVehicleResponse()
        {
            Id = vehicle.Id,
            Name = vehicle.Name!,
            Description = vehicle.Description!,
            Price = vehicle.Price,
            VehicleBrandId = vehicle.VehicleBrandId,
            VehicleOwnerId = vehicle.VehicleOwnerId,
            VehicleOwner = new GetVehicleOwnerResponse()
            {
                Id = existingVehicleOwner.Id,
                Name = existingVehicleOwner.Name!,
                Address = existingVehicleOwner.Address!
            },
            VehicleBrand = new GetVehicleBrandResponse()
            {
                Id = existingVehicleBrand.Id,
                Name = existingVehicleBrand.Name!,
                Location = existingVehicleBrand.Location!
            }
        };

        logger.LogInformation("Successfully retrieved vehicle details for Vehicle Id: {VehicleId}", request.Id);
        return Result.Success(result);
    }
} 