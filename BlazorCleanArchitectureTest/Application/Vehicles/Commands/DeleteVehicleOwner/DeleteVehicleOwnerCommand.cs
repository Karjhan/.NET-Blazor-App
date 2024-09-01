using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.DeleteVehicleOwner;

public sealed record DeleteVehicleOwnerCommand(
    Guid Id
) : ICommand;