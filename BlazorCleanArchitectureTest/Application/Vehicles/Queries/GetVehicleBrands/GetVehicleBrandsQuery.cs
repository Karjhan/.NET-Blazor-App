using Application.Responses.VehicleBrands;
using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Queries.GetVehicleBrands;

public sealed record GetVehicleBrandsQuery(

) : IQuery<IEnumerable<GetVehicleBrandResponse>>;