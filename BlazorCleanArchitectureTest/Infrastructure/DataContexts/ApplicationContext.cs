using System.Reflection;
using Domain.Models.Authentication;
using Domain.Models.Vehicles;
using Infrastructure.Abstractions.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataContexts;

public class ApplicationContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public DbSet<Vehicle> Vehicles { get; set; }
    
    public DbSet<VehicleBrand> VehicleBrands { get; set; }
    
    public DbSet<VehicleOwner> VehicleOwners { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    } 
}