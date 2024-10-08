﻿using Domain.Models.Vehicles;

namespace Domain.Specifications;

public class VehicleBrandByNameSpecification : BaseSpecification<VehicleBrand>
{
    public VehicleBrandByNameSpecification(string name) : base(brand => brand.Name != null && brand.Name.ToLower() == name.ToLower())
    {
        
    }
}