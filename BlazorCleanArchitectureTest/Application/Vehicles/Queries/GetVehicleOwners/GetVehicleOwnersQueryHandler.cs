using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Queries.GetVehicleOwners;

public class GetVehicleOwnersQueryHandler(
    ApplicationContext context,
    ILogger<GetVehicleOwnersQueryHandler> logger
) : IQueryHandler<GetVehicleOwnersQuery, IEnumerable<GetVehicleOwnerResponse>> 
{
    public async Task<Result<IEnumerable<GetVehicleOwnerResponse>>> Handle(GetVehicleOwnersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetVehicleOwnersQuery to retrieve all vehicle owners.");
        
        var data = await context.VehicleOwners.ToListAsync(cancellationToken);
        if (data is null)
        {
            logger.LogWarning("No vehicle owners found.");
            return new List<GetVehicleOwnerResponse>();
        }
        
        logger.LogInformation("Successfully retrieved {VehicleOwnerCount} vehicle owners.", data.Count);

        var result = data.Select(owner => new GetVehicleOwnerResponse()
        {
            Id = owner.Id,
            Name = owner.Name!,
            Address = owner.Address!
        });

        logger.LogInformation("Successfully mapped vehicle owners to response objects.");
        return Result.Success(result);
    }
}