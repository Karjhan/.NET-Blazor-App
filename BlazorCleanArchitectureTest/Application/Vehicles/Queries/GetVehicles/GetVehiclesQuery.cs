using Application.Responses.Vehicles;
using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Queries.GetVehicles;

public sealed record GetVehiclesQuery(

) : IQuery<IEnumerable<GetVehicleResponse>>;