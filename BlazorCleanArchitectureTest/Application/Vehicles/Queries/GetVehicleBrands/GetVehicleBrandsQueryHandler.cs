using Application.Responses.VehicleBrands;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Queries.GetVehicleBrands;

public class GetVehicleBrandsQueryHandler(
    ApplicationContext context,
    ILogger<GetVehicleBrandsQueryHandler> logger
) : IQueryHandler<GetVehicleBrandsQuery, IEnumerable<GetVehicleBrandResponse>>
{
    public async Task<Result<IEnumerable<GetVehicleBrandResponse>>> Handle(GetVehicleBrandsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetVehicleBrandsQuery to retrieve all vehicle brands.");
        
        var data = await context.VehicleBrands.ToListAsync(cancellationToken);
        if (data is null)
        {
            logger.LogWarning("No vehicle brands found.");
            return new List<GetVehicleBrandResponse>();
        }
        
        logger.LogInformation("Successfully retrieved {VehicleBrandCount} vehicle brands.", data.Count);

        var result = data.Select(brand => new GetVehicleBrandResponse()
        {
            Id = brand.Id,
            Name = brand.Name!,
            Location = brand.Location!
        });

        logger.LogInformation("Successfully mapped vehicle brands to response objects.");
        return Result.Success(result);
    }
}