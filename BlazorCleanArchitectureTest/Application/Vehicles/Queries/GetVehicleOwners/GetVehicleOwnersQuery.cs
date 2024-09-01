using Application.Responses.VehicleOwners;
using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Queries.GetVehicleOwners;

public sealed record GetVehicleOwnersQuery(

) : IQuery<IEnumerable<GetVehicleOwnerResponse>>;