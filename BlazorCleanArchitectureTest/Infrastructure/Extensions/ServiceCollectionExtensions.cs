using Domain.Models.Authentication;
using Infrastructure.Abstractions.Persistence;
using Infrastructure.Configurations;
using Infrastructure.DataContexts;
using Infrastructure.Repositories;
using Infrastructure.SeedData.Initializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add EF Core configuration
        services.Configure<EFConfiguration>(configuration.GetSection(nameof(EFConfiguration)).Bind);

        var efConfiguration = configuration.GetSection(nameof(EFConfiguration)).Get<EFConfiguration>();
        
        // Add generic repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        // Add Seed Data configuration
        services.Configure<SeedDataConfiguration>(configuration.GetSection(nameof(SeedDataConfiguration)).Bind);
        
        // Add Seed Data initializer
        services.AddTransient<IDataContextSeed, DataContextSeed>();
        
        // Add entity dbContext for app, add postgres connection for dbContext
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(efConfiguration.ConnectionString);
        });
        
        // Add Identity 
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddSignInManager();
        
        return services;
    }
}