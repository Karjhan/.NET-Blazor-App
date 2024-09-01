using Application.Requests.VehicleBrands;
using Application.Requests.VehicleOwners;
using Application.Requests.Vehicles;
using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;
using Application.Responses.Vehicles;
using Domain.Primitives;

namespace Application.Services;

public class VehicleService : IVehicleService
{
    public Task<Result> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<GetVehicleResponse>>> GetVehiclesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<GetVehicleResponse>> GetVehicleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateVehicleAsync(UpdateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateVehicleBrandAsync(CreateVehicleBrandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<GetVehicleBrandResponse>>> GetVehicleBrandsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<GetVehicleBrandResponse>> GetVehicleBrandAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteVehicleBrandAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateVehicleBrandAsync(UpdateVehicleBrandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateVehicleOwnerAsync(CreateVehicleOwnerRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<GetVehicleOwnerResponse>>> GetVehicleOwnerAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<GetVehicleOwnerResponse>> GetVehicleOwnerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteVehicleOwnerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateVehicleOwnerAsync(UpdateVehicleOwnerRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}