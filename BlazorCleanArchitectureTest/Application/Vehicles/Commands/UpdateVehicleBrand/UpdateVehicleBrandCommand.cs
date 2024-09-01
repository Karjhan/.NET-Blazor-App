using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.UpdateVehicleBrand;

public sealed record UpdateVehicleBrandCommand(
    Guid Id,
    string Name,
    string Location
) : ICommand;