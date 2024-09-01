using Application.Vehicles.Commands.UpdateVehicleBrand;
using Domain.Exceptions;
using Domain.Models.Vehicles;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.UpdateVehicleOwner;

public class UpdateVehicleOwnerCommandHandler(
    ApplicationContext context,
    ILogger<UpdateVehicleBrandCommandHandler> logger
) : ICommandHandler<UpdateVehicleOwnerCommand>
{
    public async Task<Result> Handle(UpdateVehicleOwnerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateVehicleOwnerCommand for VehicleOwner Id: {VehicleOwnerId}", request.Id);

        var vehicleOwner = await context.VehicleOwners.FirstOrDefaultAsync(
            owner => owner.Id == request.Id,
            cancellationToken
        );
        if (vehicleOwner is null)
        {
            logger.LogWarning("VehicleOwner with Id: {VehicleOwnerId} not found.", request.Id);
            return Result.Failure(Error.VehicleOwnerNotFound);
        }

        logger.LogInformation("VehicleOwner with Id: {VehicleOwnerId} found. Proceeding with update.", request.Id);

        context.Entry(vehicleOwner).State = EntityState.Detached;
        var updatedVehicleOwner = VehicleOwner.Create(
            vehicleOwner.Id,
            request.Name,
            request.Address,
            vehicleOwner.Vehicles!
        );
        context.VehicleOwners.Update(updatedVehicleOwner);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated VehicleOwner with Id: {VehicleOwnerId}.", request.Id);
        return Result.Success();
    }
}