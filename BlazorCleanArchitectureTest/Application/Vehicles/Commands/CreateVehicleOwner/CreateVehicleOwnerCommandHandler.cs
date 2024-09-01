using Domain.Exceptions;
using Domain.Models.Vehicles;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.CreateVehicleOwner;

public class CreateVehicleOwnerCommandHandler(
    ApplicationContext context,
    ILogger<CreateVehicleOwnerCommandHandler> logger
): ICommandHandler<CreateVehicleOwnerCommand>
{
    public async Task<Result> Handle(CreateVehicleOwnerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling CreateVehicleOwnerCommand for VehicleOwner Name: {VehicleOwnerName}, Address: {Address}",
            request.Name, request.Address);
        
        var vehicleOwner = await context.VehicleOwners.FirstOrDefaultAsync(
            owner => owner.Name == request.Name,
            cancellationToken
        );
        if (vehicleOwner is not null)
        {
            logger.LogWarning("VehicleOwner with Name: {VehicleOwnerName} already exists.", request.Name);
            return Result.Failure(Error.VehicleOwnerAlreadyExists);
        }

        var newVehicleOwner = VehicleOwner.Create(
            Guid.NewGuid(),
            request.Name,
            request.Address,
            new List<Vehicle>()
        );
        context.VehicleOwners.Add(newVehicleOwner);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created VehicleOwner with Id: {VehicleOwnerId}, Name: {VehicleOwnerName}", 
            newVehicleOwner.Id, request.Name);
        return Result.Success();
    }
}