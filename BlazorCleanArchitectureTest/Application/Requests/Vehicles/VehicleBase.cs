using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Vehicles;

public class VehicleBase
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Description { get; set; } = null!;
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public Guid VehicleOwnerId { get; set; }
    
    [Required]
    public Guid VehicleBrandId { get; set; }
}