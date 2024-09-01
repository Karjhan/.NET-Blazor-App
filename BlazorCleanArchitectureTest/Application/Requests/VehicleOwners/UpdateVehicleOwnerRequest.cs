using System.ComponentModel.DataAnnotations;

namespace Application.Requests.VehicleOwners;

public class UpdateVehicleOwnerRequest : CreateVehicleOwnerRequest
{
    [Required]
    public Guid Id { get; set; }
}