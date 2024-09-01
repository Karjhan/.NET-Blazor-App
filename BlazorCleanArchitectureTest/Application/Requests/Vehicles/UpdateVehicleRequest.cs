using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Vehicles;

public class UpdateVehicleRequest : CreateVehicleRequest
{
    [Required]
    public Guid Id { get; set; }
}