using Domain.Exceptions;
using Domain.Models.Vehicles;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.Abstractions.Persistence;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler(
    ApplicationContext context,
    IGenericRepository<Vehicle> vehicleRepository,
    ILogger<CreateVehicleCommandHandler> logger
) : ICommandHandler<CreateVehicleCommand>
{
    public async Task<Result> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling CreateVehicleCommand for Vehicle Name: {VehicleName}, VehicleOwnerId: {VehicleOwnerId}, VehicleBrandId: {VehicleBrandId}",
            request.Name, request.VehicleOwnerId, request.VehicleBrandId);
        
        var vehicle = await context.Vehicles.FirstOrDefaultAsync(
            vehicle => vehicle.Name!.ToLower() == request.Name.ToLower(), 
            cancellationToken);
        if (vehicle is not null)
        {
            logger.LogWarning("Vehicle with Name: {VehicleName} already exists.", request.Name);
            return Result.Failure(Error.VehicleAlreadyExists);
        }

        var existingVehicleOwner = await context.VehicleOwners.FirstOrDefaultAsync(
            owner => owner.Id == request.VehicleOwnerId,
            cancellationToken
        );
        if (existingVehicleOwner is null)
        {
            logger.LogWarning("VehicleOwner with Id: {VehicleOwnerId} not found.", request.VehicleOwnerId);
            return Result.Failure(Error.VehicleOwnerNotFound);
        }
        
        var existingVehicleBrand = await context.VehicleBrands.FirstOrDefaultAsync(
            brand => brand.Id == request.VehicleBrandId,
            cancellationToken
        );
        if (existingVehicleBrand is null)
        {
            logger.LogWarning("VehicleBrand with Id: {VehicleBrandId} not found.", request.VehicleBrandId);
            return Result.Failure(Error.VehicleBrandNotFound);
        }

        var newVehicle = Vehicle.Create(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.Price,
            existingVehicleOwner,
            request.VehicleOwnerId,
            existingVehicleBrand,
            request.VehicleBrandId
        );
        context.Vehicles.Add(newVehicle);
        
        // existingVehicleOwner.Vehicles!.Add(newVehicle);
        // context.VehicleOwners.Update(existingVehicleOwner);
        //
        // existingVehicleBrand.Vehicles!.Add(newVehicle);
        // context.VehicleBrands.Update(existingVehicleBrand);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created Vehicle with Id: {VehicleId}, Name: {VehicleName}", newVehicle.Id, request.Name);
        return Result.Success();
    }
}