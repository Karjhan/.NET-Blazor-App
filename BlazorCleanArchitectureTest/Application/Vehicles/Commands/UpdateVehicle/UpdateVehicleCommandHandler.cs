using Domain.Exceptions;
using Domain.Models.Vehicles;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler(
    ApplicationContext context,
    ILogger<UpdateVehicleCommandHandler> logger
) : ICommandHandler<UpdateVehicleCommand>
{
    public async Task<Result> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling UpdateVehicleCommand for Vehicle Id: {VehicleId}", request.Id);
        
        var vehicle = await context.Vehicles.FirstOrDefaultAsync(
            vehicle => vehicle.Id == request.Id,
            cancellationToken
        );
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle with Id: {VehicleId} not found.", request.Id);
            return Result.Failure(Error.VehicleNotFound);
        }
        
        logger.LogInformation("Vehicle with Id: {VehicleId} found. Proceeding with validation checks.", request.Id);
        
        var existingVehicleOwner = await context.VehicleOwners.FirstOrDefaultAsync(
            owner => owner.Id == request.VehicleOwnerId,
            cancellationToken
        );
        if (existingVehicleOwner is null)
        {
            logger.LogWarning("VehicleOwner with Id: {VehicleOwnerId} not found for Vehicle Id: {VehicleId}.", request.VehicleOwnerId, request.Id);
            return Result.Failure(Error.VehicleOwnerNotFound);
        }
        
        var existingVehicleBrand = await context.VehicleBrands.FirstOrDefaultAsync(
            brand => brand.Id == request.VehicleBrandId,
            cancellationToken
        );
        if (existingVehicleBrand is null)
        {
            logger.LogWarning("VehicleBrand with Id: {VehicleBrandId} not found for Vehicle Id: {VehicleId}.", request.VehicleBrandId, request.Id);
            return Result.Failure(Error.VehicleBrandNotFound);
        }
        
        logger.LogInformation("Updating Vehicle with Id: {VehicleId}.", request.Id);

        context.Entry(vehicle).State = EntityState.Detached;
        var updatedVehicle = Vehicle.Create(
            vehicle.Id,
            request.Name,
            request.Description,
            request.Price,
            existingVehicleOwner,
            request.VehicleOwnerId,
            existingVehicleBrand,
            request.VehicleBrandId
        );
        context.Vehicles.Update(updatedVehicle);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated Vehicle with Id: {VehicleId}.", request.Id);
        return Result.Success();
    }
}