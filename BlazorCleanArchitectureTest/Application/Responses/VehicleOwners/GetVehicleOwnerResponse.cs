using Application.Responses.Vehicles;

namespace Application.Responses.VehicleOwners;

public class GetVehicleOwnerResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Address { get; set; } = null!;

    public virtual ICollection<GetVehicleResponse> Vehicles { get; set; } = null!;
}