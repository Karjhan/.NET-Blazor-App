using Application.Responses.VehicleBrands;
using Infrastructure.Abstractions.CQRS;

namespace Application.Vehicles.Queries.GetSingleVehicleBrand;

public sealed record GetSingleVehicleBrandQuery(
    Guid Id
) : IQuery<GetVehicleBrandResponse>;