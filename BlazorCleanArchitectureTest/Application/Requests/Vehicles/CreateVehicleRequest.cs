using Application.Vehicles.Commands.CreateVehicle;

namespace Application.Requests.Vehicles;

public class CreateVehicleRequest : VehicleBase
{
    public CreateVehicleCommand ToCreateVehicleCommand()
    {
        CreateVehicleCommand createVehicleCommand = new CreateVehicleCommand(
            Name, 
            Description,
            Price,
            VehicleOwnerId,
            VehicleBrandId
        );

        return createVehicleCommand;
    }
}
