using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Commands.CreateVehicleBrand;

public sealed record CreateVehicleBrandCommand(
    string Name,
    string Location
) : ICommand;