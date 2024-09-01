using Application.Responses.Vehicles;
using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Queries.GetSingleVehicle;

public sealed record GetSingleVehicleQuery(
    Guid Id
) : IQuery<GetVehicleResponse>;