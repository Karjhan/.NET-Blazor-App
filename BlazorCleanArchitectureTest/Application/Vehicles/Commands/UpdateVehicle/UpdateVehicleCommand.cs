using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.UpdateVehicle;

public sealed record UpdateVehicleCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid VehicleOwnerId,
    Guid VehicleBrandId
) : ICommand;