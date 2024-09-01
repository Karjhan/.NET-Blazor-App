using Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EntitiesConfiguration;

public class VehicleBrandConfiguration: IEntityTypeConfiguration<VehicleBrand>
{
    public void Configure(EntityTypeBuilder<VehicleBrand> builder)
    {
        builder.ToTable("VehicleBrands");

        builder.HasKey(vehicleBrand => vehicleBrand.Id);
        
        builder.HasMany(vehicleBrand => vehicleBrand.Vehicles).WithOne(vehicle => vehicle.VehicleBrand).OnDelete(DeleteBehavior.Cascade).HasForeignKey(vehicle => vehicle.VehicleBrandId).IsRequired();
    }
}