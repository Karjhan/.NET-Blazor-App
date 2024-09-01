using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.UpdateVehicleOwner;

public sealed record UpdateVehicleOwnerCommand(
    Guid Id,
    string Name,
    string Address
) : ICommand;