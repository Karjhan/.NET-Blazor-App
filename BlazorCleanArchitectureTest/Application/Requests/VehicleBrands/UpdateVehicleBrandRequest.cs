using System.ComponentModel.DataAnnotations;
using Application.Vehicles.Commands.UpdateVehicleBrand;

namespace Application.Requests.VehicleBrands;

public class UpdateVehicleBrandRequest : CreateVehicleBrandRequest
{
    [Required]
    public Guid Id { get; set; }
    
    public UpdateVehicleBrandCommand ToUpdateVehicleBrandCommand()
    {
        UpdateVehicleBrandCommand updateVehicleBrandCommand = new UpdateVehicleBrandCommand(
            Id,
            Name,
            Location
        );

        return updateVehicleBrandCommand;
    }
}