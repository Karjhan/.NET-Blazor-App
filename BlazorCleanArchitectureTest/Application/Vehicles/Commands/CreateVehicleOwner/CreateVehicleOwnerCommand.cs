using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.CreateVehicleOwner;

public sealed record CreateVehicleOwnerCommand(
    string Name,
    string Address
) : ICommand;