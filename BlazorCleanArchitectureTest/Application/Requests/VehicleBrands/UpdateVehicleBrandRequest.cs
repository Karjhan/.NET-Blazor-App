using System.ComponentModel.DataAnnotations;

namespace Application.Requests.VehicleBrands;

public class UpdateVehicleBrandRequest : CreateVehicleBrandRequest
{
    [Required]
    public Guid Id { get; set; }
}