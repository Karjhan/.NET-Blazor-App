using Application.Accounts.Commands.CreateAccount;
using Domain.Exceptions;
using Domain.Models.Vehicles;
using Domain.Primitives;
using Infrastructure.Abstractions.CQRS;
using Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Vehicles.Commands.CreateVehicleBrand;

public class CreateVehicleBrandCommandHandler(
    ApplicationContext context,
    ILogger<CreateVehicleBrandCommandHandler> logger
) : ICommandHandler<CreateVehicleBrandCommand>
{
    public async Task<Result> Handle(CreateVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling CreateVehicleBrandCommand for VehicleBrand Name: {VehicleBrandName}, Location: {Location}",
            request.Name, request.Location);
        
        var vehicleBrand = await context.VehicleBrands.FirstOrDefaultAsync(
            brand => brand.Name == request.Name,
            cancellationToken
        );
        if (vehicleBrand is not null)
        {
            logger.LogWarning("VehicleBrand with Name: {VehicleBrandName} already exists.", request.Name);
            return Result.Failure(Error.VehicleBrandAlreadyExists);
        }

        var newVehicleBrand = VehicleBrand.Create(
            Guid.NewGuid(),
            request.Name,
            request.Location,
            new List<Vehicle>()
        );
        context.VehicleBrands.Add(newVehicleBrand);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created VehicleBrand with Id: {VehicleBrandId}, Name: {VehicleBrandName}", 
            newVehicleBrand.Id, request.Name);
        return Result.Success();
    }
}