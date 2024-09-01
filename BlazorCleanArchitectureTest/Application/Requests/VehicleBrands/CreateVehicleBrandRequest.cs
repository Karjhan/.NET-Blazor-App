using System.ComponentModel.DataAnnotations;

namespace Application.Requests.VehicleBrands;

public class CreateVehicleBrandRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Location { get; set; } = null!;
}