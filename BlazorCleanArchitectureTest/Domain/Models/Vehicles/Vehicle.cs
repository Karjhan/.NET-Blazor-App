using Domain.Primitives;

namespace Domain.Models.Vehicles;

public class Vehicle : Entity
{
    public string? Name { get; private set; }

    public string? Description { get; private set; }
    
    public decimal Price { get; private set; }

    public VehicleOwner? VehicleOwner { get; private set; }

    public Guid VehicleOwnerId { get; private set; }

    public VehicleBrand? VehicleBrand { get; private set; }

    public Guid VehicleBrandId { get; private set; }
    
    public Vehicle(Guid id) : base(id)
    {
        
    }

    private Vehicle(
        Guid id, 
        string name, 
        string description, 
        decimal price, 
        VehicleOwner vehicleOwner, 
        Guid vehicleOwnerId, 
        VehicleBrand vehicleBrand, 
        Guid vehicleBrandId
    ) : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
        VehicleOwner = vehicleOwner;
        VehicleOwnerId = vehicleOwnerId;
        VehicleBrand = vehicleBrand;
        VehicleBrandId = vehicleBrandId;
    }

    public static Vehicle Create(
        Guid id, 
        string name, 
        string description, 
        decimal price, 
        VehicleOwner vehicleOwner, 
        Guid vehicleOwnerId, 
        VehicleBrand vehicleBrand, 
        Guid vehicleBrandId
    )
    {
        Vehicle vehicle = new Vehicle(
            id, 
            name, 
            description, 
            price, 
            vehicleOwner, 
            vehicleOwnerId, 
            vehicleBrand, 
            vehicleBrandId
        );

        return vehicle;
    }
}