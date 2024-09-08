using Domain.Models.Vehicles;

namespace Domain.Specifications;

public class VehicleByNameSpecification : BaseSpecification<Vehicle>
{
    public VehicleByNameSpecification(string name) : base(vehicle => vehicle.Name != null && vehicle.Name.ToLower() == name.ToLower())
    {
        AddInclude(vehicle => vehicle.VehicleBrand!);
        AddInclude(vehicle => vehicle.VehicleOwner!);
    }
}