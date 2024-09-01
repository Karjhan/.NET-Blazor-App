using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.DeleteVehicleOwner;

public class DeleteVehicleOwnerCommandHandler(
    ApplicationContext context,
    ILogger<DeleteVehicleOwnerCommandHandler> logger
) : ICommandHandler<DeleteVehicleOwnerCommand>
{
    public async Task<Result> Handle(DeleteVehicleOwnerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteVehicleOwnerCommand for VehicleOwner Id: {VehicleOwnerId}", request.Id);
        
        var vehicleOwner = await context.VehicleOwners.FirstOrDefaultAsync(
            owner => owner.Id == request.Id,
            cancellationToken
        );
        if (vehicleOwner is null)
        {
            logger.LogWarning("VehicleOwner with Id: {VehicleOwnerId} not found.", request.Id);
            return Result.Failure(Error.VehicleOwnerNotFound);
        }

        context.VehicleOwners.Remove(vehicleOwner);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted VehicleOwner with Id: {VehicleOwnerId}", request.Id);
        return Result.Success();
    }
}

