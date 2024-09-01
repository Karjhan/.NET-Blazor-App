using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.CreateVehicle;

public sealed record CreateVehicleCommand(
    string Name,
    string Description,
    decimal Price,
    Guid VehicleOwnerId,
    Guid VehicleBrandId
) : ICommand;