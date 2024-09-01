using System.Text.Json.Serialization;
using Domain.Primitives;

namespace Domain.Models.Vehicles;

public class VehicleOwner : Entity
{
    public string? Name { get; private set; }

    public string? Address { get; private set; }
    
    [JsonIgnore]
    public ICollection<Vehicle>? Vehicles { get; private set; }
    
    public VehicleOwner(Guid id) : base(id)
    {
        
    }

    private VehicleOwner(
        Guid id, 
        string name, 
        string address, 
        ICollection<Vehicle> vehicles
    ) : base(id)
    {
        Name = name;
        Address = address;
        Vehicles = vehicles;
    }

    public static VehicleOwner Create(
        Guid id, 
        string name, 
        string address, 
        ICollection<Vehicle> vehicles
    )
    {
        VehicleOwner vehicleOwner = new VehicleOwner(
            id, 
            name, 
            address,
            vehicles
        );

        return vehicleOwner;
    }
}