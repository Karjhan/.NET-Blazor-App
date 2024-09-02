using System.ComponentModel.DataAnnotations;
using Application.Vehicles.Commands.CreateVehicleOwner;

namespace Application.Requests.VehicleOwners;

public class CreateVehicleOwnerRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
    
    public CreateVehicleOwnerCommand ToCreateVehicleOwnerCommand()
    {
        CreateVehicleOwnerCommand createAccountCommand = new CreateVehicleOwnerCommand(
            Name,
            Address
        );

        return createAccountCommand;
    }
}