using Application.Requests.VehicleBrands;
using Application.Requests.VehicleOwners;
using Application.Requests.Vehicles;
using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;
using Application.Responses.Vehicles;
using Domain.Primitives;

namespace Application.Services;

public interface IVehicleService
{
    // Vehicle
    
    Task<Result> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<GetVehicleResponse>>> GetVehiclesAsync(CancellationToken cancellationToken = default);

    Task<Result<GetVehicleResponse>> GetVehicleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> UpdateVehicleAsync(UpdateVehicleRequest request, CancellationToken cancellationToken = default);
    
    // Vehicle Brand
    
    Task<Result> CreateVehicleBrandAsync(CreateVehicleBrandRequest request, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<GetVehicleBrandResponse>>> GetVehicleBrandsAsync(CancellationToken cancellationToken = default);

    Task<Result<GetVehicleBrandResponse>> GetVehicleBrandAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteVehicleBrandAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> UpdateVehicleBrandAsync(UpdateVehicleBrandRequest request, CancellationToken cancellationToken = default);
    
    // Vehicle Owner
    
    Task<Result> CreateVehicleOwnerAsync(CreateVehicleOwnerRequest request, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<GetVehicleOwnerResponse>>> GetVehicleOwnersAsync(CancellationToken cancellationToken = default);

    Task<Result<GetVehicleOwnerResponse>> GetVehicleOwnerAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> DeleteVehicleOwnerAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> UpdateVehicleOwnerAsync(UpdateVehicleOwnerRequest request, CancellationToken cancellationToken = default);
}