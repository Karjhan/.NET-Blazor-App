using Application.Responses.VehicleBrands;
using Application.Responses.Vehicles;
using Application.Vehicles.Queries.GetSingleVehicle;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Queries.GetSingleVehicleBrand;

public class GetSingleVehicleBrandQueryHandler(
    ApplicationContext context,
    ILogger<GetSingleVehicleBrandQueryHandler> logger
) : IQueryHandler<GetSingleVehicleBrandQuery, GetVehicleBrandResponse>
{
    public async Task<Result<GetVehicleBrandResponse>> Handle(GetSingleVehicleBrandQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetSingleVehicleBrandQuery for Vehicle Brand Id: {VehicleBrandId}", request.Id);
        
        var vehicleBrand = await context.VehicleBrands.FirstOrDefaultAsync(
            brand => brand.Id == request.Id,
            cancellationToken
        );
        if (vehicleBrand is null)
        {
            logger.LogWarning("Vehicle Brand with Id: {VehicleBrandId} not found.", request.Id);
            return Result.Failure<GetVehicleBrandResponse>(Error.VehicleBrandNotFound);
        }

        logger.LogInformation("Vehicle Brand with Id: {VehicleBrandId} found. Preparing response.", request.Id);
        
        var result = new GetVehicleBrandResponse()
        {
            Id = vehicleBrand.Id,
            Name = vehicleBrand.Name!,
            Location = vehicleBrand.Location!,
        };

        logger.LogInformation("Successfully retrieved vehicle brand details for Vehicle Brand Id: {VehicleBrandId}", request.Id);
        return Result.Success(result);
    }
} 