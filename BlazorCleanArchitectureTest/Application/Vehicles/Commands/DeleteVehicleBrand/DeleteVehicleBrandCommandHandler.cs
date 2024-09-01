using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.DeleteVehicleBrand;

public class DeleteVehicleBrandCommandHandler(
    ApplicationContext context,
    ILogger<DeleteVehicleBrandCommandHandler> logger
) : ICommandHandler<DeleteVehicleBrandCommand>
{
    public async Task<Result> Handle(DeleteVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteVehicleBrandCommand for VehicleBrand Id: {VehicleBrandId}", request.Id);
        
        var vehicleBrand = await context.VehicleBrands.FirstOrDefaultAsync(
            brand => brand.Id == request.Id,
            cancellationToken
        );
        if (vehicleBrand is null)
        {
            logger.LogWarning("VehicleBrand with Id: {VehicleBrandId} not found.", request.Id);
            return Result.Failure(Error.VehicleBrandNotFound);
        }

        context.VehicleBrands.Remove(vehicleBrand);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted VehicleBrand with Id: {VehicleBrandId}", request.Id);
        return Result.Success();
    }
}