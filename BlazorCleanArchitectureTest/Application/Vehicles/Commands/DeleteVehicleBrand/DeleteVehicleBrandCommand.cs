using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.DeleteVehicleBrand;

public sealed record DeleteVehicleBrandCommand(
    Guid Id
) : ICommand;