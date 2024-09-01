using Application.Responses.Vehicles;

namespace Application.Responses.VehicleBrands;

public class GetVehicleBrandResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Location { get; set; } = null!;

    public virtual ICollection<GetVehicleResponse> Vehicles { get; set; } = null!;
}