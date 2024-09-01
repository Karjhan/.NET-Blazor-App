using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler(
    ApplicationContext context,
    ILogger<DeleteVehicleCommandHandler> logger
) : ICommandHandler<DeleteVehicleCommand>
{
    public async Task<Result> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteVehicleCommand for Vehicle Id: {VehicleId}", request.Id);
        
        var vehicle = await context.Vehicles.FirstOrDefaultAsync(
            vehicle => vehicle.Id == request.Id,
            cancellationToken
        );
        if (vehicle is null)
        {
            logger.LogWarning("Vehicle with Id: {VehicleId} not found.", request.Id);
            return Result.Failure(Error.VehicleNotFound);
        }

        context.Vehicles.Remove(vehicle);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted Vehicle with Id: {VehicleId}", request.Id);
        return Result.Success();
    }
}