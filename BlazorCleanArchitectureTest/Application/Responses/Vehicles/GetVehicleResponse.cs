namespace Application.Responses.Vehicles;

public class GetVehicleResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public decimal Price { get; set; }
    
    public Guid VehicleOwnerId { get; set; }
    
    public Guid VehicleBrandId { get; set; }
}