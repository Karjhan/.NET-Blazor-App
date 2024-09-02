using System.ComponentModel.DataAnnotations;
using Application.Vehicles.Commands.CreateVehicleBrand;

namespace Application.Requests.VehicleBrands;

public class CreateVehicleBrandRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Location { get; set; } = null!;
    
    public CreateVehicleBrandCommand ToCreateVehicleBrandCommand()
    {
        CreateVehicleBrandCommand createVehicleBrandCommand = new CreateVehicleBrandCommand(
            Name,
            Location
        );

        return createVehicleBrandCommand;
    }
}