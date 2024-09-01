using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.DeleteVehicle;

public sealed record DeleteVehicleCommand(
    Guid Id
) : ICommand;