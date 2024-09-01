using Application.Responses.VehicleBrands;
using Application.Responses.VehicleOwners;

namespace Application.Responses.Vehicles;

public class GetVehicleResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public decimal Price { get; set; }
    
    public Guid VehicleOwnerId { get; set; }

    public GetVehicleOwnerResponse VehicleOwner { get; set; } = null!;

    public Guid VehicleBrandId { get; set; }

    public GetVehicleBrandResponse VehicleBrand { get; set; } = null!;
}