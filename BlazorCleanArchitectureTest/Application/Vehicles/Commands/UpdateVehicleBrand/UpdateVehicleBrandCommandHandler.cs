using Domain.Exceptions;
using Domain.Models.Vehicles;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.UpdateVehicleBrand;

public class UpdateVehicleBrandCommandHandler(
    ApplicationContext context,
    ILogger<UpdateVehicleBrandCommandHandler> logger
) : ICommandHandler<UpdateVehicleBrandCommand>
{
    public async Task<Result> Handle(UpdateVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateVehicleBrandCommand for VehicleBrand Id: {VehicleBrandId}", request.Id);
        
        var vehicleBrand = await context.VehicleBrands.FirstOrDefaultAsync(
            brand => brand.Id == request.Id,
            cancellationToken
        );
        if (vehicleBrand is null)
        {
            logger.LogWarning("VehicleBrand with Id: {VehicleBrandId} not found.", request.Id);
            return Result.Failure(Error.VehicleBrandNotFound);
        }
        
        logger.LogInformation("VehicleBrand with Id: {VehicleBrandId} found. Proceeding with update.", request.Id);

        context.Entry(vehicleBrand).State = EntityState.Detached;
        var updatedVehicleBrand = VehicleBrand.Create(
            vehicleBrand.Id,
            request.Name,
            request.Location,
            vehicleBrand.Vehicles!
        );
        context.VehicleBrands.Update(updatedVehicleBrand);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated VehicleBrand with Id: {VehicleBrandId}.", request.Id);
        return Result.Success();
    }
}