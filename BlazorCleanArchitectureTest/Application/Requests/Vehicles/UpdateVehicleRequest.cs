using System.ComponentModel.DataAnnotations;
using Application.Vehicles.Commands.UpdateVehicle;

namespace Application.Requests.Vehicles;

public class UpdateVehicleRequest : CreateVehicleRequest
{
    [Required]
    public Guid Id { get; set; }
    
    public UpdateVehicleCommand ToUpdateVehicleCommand()
    {
        UpdateVehicleCommand updateVehicleCommand = new UpdateVehicleCommand(
            Id,
            Name, 
            Description,
            Price,
            VehicleOwnerId,
            VehicleBrandId
        );

        return updateVehicleCommand;
    }
}