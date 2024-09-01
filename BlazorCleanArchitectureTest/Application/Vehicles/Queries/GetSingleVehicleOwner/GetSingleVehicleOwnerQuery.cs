using Application.Responses.VehicleOwners;
using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Queries.GetSingleVehicleOwner;

public sealed record GetSingleVehicleOwnerQuery(
    Guid Id
) : IQuery<GetVehicleOwnerResponse>;