using Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EntitiesConfiguration;

public class VehicleConfiguration: IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(vehicle => vehicle.Id);
        
        builder.HasOne(vehicle => vehicle.VehicleBrand).WithMany(brand => brand.Vehicles).HasForeignKey(vehicle => vehicle.VehicleBrandId);
        
        builder.HasOne(vehicle => vehicle.VehicleOwner).WithMany(owner => owner.Vehicles).HasForeignKey(vehicle => vehicle.VehicleOwnerId);
    }
}