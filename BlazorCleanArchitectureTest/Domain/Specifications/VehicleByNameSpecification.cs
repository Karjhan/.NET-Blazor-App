using Domain.Models.Vehicles;

namespace Domain.Specifications;

public class VehicleByNameSpecification : BaseSpecification<Vehicle>
{
    public VehicleByNameSpecification(string name) : base(vehicle => vehicle.Name == name)
    {
        AddInclude(vehicle => vehicle.VehicleBrand!);
        AddInclude(vehicle => vehicle.VehicleOwner!);
    }
}