using System.Text.Json.Serialization;
using Domain.Primitives;

namespace Domain.Models.Vehicles;

public class VehicleBrand : Entity
{
    public string? Name { get; private set; }

    public string? Location { get; private set; }
    
    [JsonIgnore]
    public ICollection<Vehicle>? Vehicles { get; private set; }
    
    public VehicleBrand(Guid id) : base(id)
    {
        
    }

    private VehicleBrand(
        Guid id, 
        string name, 
        string location, 
        ICollection<Vehicle> vehicles
    ) : base(id)
    {
        Name = name;
        Location = location;
        Vehicles = vehicles;
    }

    public static VehicleBrand Create(
        Guid id, 
        string name, 
        string location, 
        ICollection<Vehicle> vehicles
    )
    {
        VehicleBrand vehicleBrand = new VehicleBrand(
            id, 
            name, 
            location,
            vehicles
        );

        return vehicleBrand;
    }
}