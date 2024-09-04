using Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.EntitiesConfiguration;

public class VehicleOwnerConfiguration: IEntityTypeConfiguration<VehicleOwner>
{
    public void Configure(EntityTypeBuilder<VehicleOwner> builder)
    {
        builder.ToTable("VehicleOwners");

        builder.HasKey(vehicleOwner => vehicleOwner.Id);
        
        builder.HasMany(vehicleOwner => vehicleOwner.Vehicles).WithOne(vehicle => vehicle.VehicleOwner).OnDelete(DeleteBehavior.Cascade).HasForeignKey(vehicle => vehicle.VehicleOwnerId).IsRequired();
    }
}