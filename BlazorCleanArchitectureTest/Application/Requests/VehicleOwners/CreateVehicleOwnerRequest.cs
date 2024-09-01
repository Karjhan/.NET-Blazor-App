using System.ComponentModel.DataAnnotations;

namespace Application.Requests.VehicleOwners;

public class CreateVehicleOwnerRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
}