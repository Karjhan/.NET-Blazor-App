using System.ComponentModel.DataAnnotations;
using Application.Vehicles.Commands.UpdateVehicleOwner;

namespace Application.Requests.VehicleOwners;

public class UpdateVehicleOwnerRequest : CreateVehicleOwnerRequest
{
    [Required]
    public Guid Id { get; set; }
    
    public UpdateVehicleOwnerCommand ToUpdateVehicleOwnerCommand()
    {
        UpdateVehicleOwnerCommand updateVehicleOwnerCommand = new UpdateVehicleOwnerCommand(
            Id,
            Name,
            Address
        );

        return updateVehicleOwnerCommand;
    }
}