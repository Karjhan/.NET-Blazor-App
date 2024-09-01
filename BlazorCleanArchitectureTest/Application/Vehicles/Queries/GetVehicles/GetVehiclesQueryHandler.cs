using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;
using Application.Responses.Vehicles;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Queries.GetVehicles;

public class GetVehiclesQueryHandler(
    ApplicationContext context,
    ILogger<GetVehiclesQueryHandler> logger
) : IQueryHandler<GetVehiclesQuery, IEnumerable<GetVehicleResponse>>
{
    public async Task<Result<IEnumerable<GetVehicleResponse>>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetVehiclesQuery to retrieve all vehicles.");
        
        var data = await context.Vehicles
            .Include(vehicle => vehicle.VehicleBrand)
            .Include(vehicle => vehicle.VehicleOwner)
            .ToListAsync(cancellationToken);
        if (data is null)
        {
            logger.LogWarning("No vehicles found.");
            return new List<GetVehicleResponse>();
        }
        
        logger.LogInformation("Successfully retrieved {VehicleCount} vehicles.", data.Count);

        var result = data.Select(vehicle => new GetVehicleResponse()
        {
            Id = vehicle.Id,
            Name = vehicle.Name!,
            Description = vehicle.Description!,
            Price = vehicle.Price,
            VehicleBrandId = vehicle.VehicleBrandId,
            VehicleOwnerId = vehicle.VehicleOwnerId,
            VehicleOwner = new GetVehicleOwnerResponse()
            {
                Id = vehicle.VehicleOwner!.Id,
                Name = vehicle.VehicleOwner.Name!,
                Address = vehicle.VehicleOwner.Address!
            },
            VehicleBrand = new GetVehicleBrandResponse()
            {
                Id = vehicle.VehicleBrand!.Id,
                Name = vehicle.VehicleBrand.Name!,
                Location = vehicle.VehicleBrand.Location!
            }
        });
        
        logger.LogInformation("Successfully mapped vehicle details to response objects.");
        return Result.Success(result);
    }
}