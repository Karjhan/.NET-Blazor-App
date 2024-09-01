using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Queries.GetSingleVehicleOwner;

public class GetSingleVehicleOwnerQueryHandler(
    ApplicationContext context,
    ILogger<GetSingleVehicleOwnerQueryHandler> logger
) : IQueryHandler<GetSingleVehicleOwnerQuery, GetVehicleOwnerResponse>
{
    public async Task<Result<GetVehicleOwnerResponse>> Handle(GetSingleVehicleOwnerQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetSingleVehicleOwnerQuery for Vehicle Owner Id: {VehicleOwnerId}", request.Id);
        
        var vehicleOwner = await context.VehicleOwners.FirstOrDefaultAsync(
            owner => owner.Id == request.Id,
            cancellationToken
        );
        if (vehicleOwner is null)
        {
            logger.LogWarning("Vehicle Owner with Id: {VehicleOwnerId} not found.", request.Id);
            return Result.Failure<GetVehicleOwnerResponse>(Error.VehicleOwnerNotFound);
        }

        logger.LogInformation("Vehicle Owner with Id: {VehicleOwnerId} found. Preparing response.", request.Id);
        
        var result = new GetVehicleOwnerResponse()
        {
            Id = vehicleOwner.Id,
            Name = vehicleOwner.Name!,
            Address = vehicleOwner.Address!
        };

        logger.LogInformation("Successfully retrieved vehicle owner details for Vehicle Owner Id: {VehicleOwnerId}", request.Id);
        return Result.Success(result);
    }
}