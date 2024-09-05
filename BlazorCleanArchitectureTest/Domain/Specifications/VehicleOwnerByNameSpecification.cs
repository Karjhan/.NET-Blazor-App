using Domain.Models.Vehicles;

namespace Domain.Specifications;

public class VehicleOwnerByNameSpecification : BaseSpecification<VehicleOwner>
{
    public VehicleOwnerByNameSpecification(string name) : base(owner => owner.Name == name)
    {
        
    }
}