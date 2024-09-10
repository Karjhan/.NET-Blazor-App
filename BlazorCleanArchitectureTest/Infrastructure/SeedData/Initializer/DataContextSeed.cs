using System.Reflection;
using System.Text.Json;
using Domain.Models.Authentication;
using Domain.Models.Vehicles;
using Infrastructure.Configurations;
using Infrastructure.Constants;
using Infrastructure.DataContexts;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.SeedData.Initializer;

public class DataContextSeed(IOptions<SeedDataConfiguration> seedOptions) : IDataContextSeed
{
    private readonly SeedDataConfiguration _options = seedOptions.Value ?? throw new ArgumentException("Invalid seed options");
    
    public async Task SeedAsync(ApplicationContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IHostEnvironment environment)
    {
        string mainPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + _options.BaseDirectory;

        List<Vehicle> vehicles = new List<Vehicle>();
        List<VehicleBrand> brands = new List<VehicleBrand>();
        List<VehicleOwner> owners = new List<VehicleOwner>();
        
        // Seed the brands
        if (!context.VehicleBrands.Any())
        {
            string vehicleBrandsResource = await File.ReadAllTextAsync(mainPath + _options.Brands);
            try
            {
                // Get data from json
                JsonDocument vehicleBrandsDocument = JsonDocument.Parse(vehicleBrandsResource);
                foreach (var vehicleBrandElement in vehicleBrandsDocument.RootElement.EnumerateArray())
                {
                    // Get data from json
                    VehicleBrandData vehicleBrandData = new VehicleBrandData();
                    vehicleBrandData.Id =
                        vehicleBrandElement.TryGetProperty(nameof(vehicleBrandData.Id), out var vehicleBrandIdValue)
                            ? vehicleBrandIdValue.ToString()
                            : default;
                    vehicleBrandData.Name = (vehicleBrandElement.TryGetProperty(nameof(vehicleBrandData.Name),
                        out var vehicleBrandNameValue)
                        ? vehicleBrandNameValue.ToString()
                        : default)!;
                    vehicleBrandData.Location = (vehicleBrandElement.TryGetProperty(nameof(vehicleBrandData.Location),
                        out var vehicleBrandLocationValue)
                        ? vehicleBrandLocationValue.ToString()
                        : default)!;

                    // Establish entity
                    VehicleBrand result = VehicleBrand.Create(
                        new Guid(vehicleBrandData.Id),
                        vehicleBrandData.Name,
                        vehicleBrandData.Location,
                        new List<Vehicle>()
                    );
                    brands.Add(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Seed the owners
        if (!context.VehicleOwners.Any())
        {
            string vehicleOwnersResource = await File.ReadAllTextAsync(mainPath + _options.Owners);
            try
            {
                // Get data from json
                JsonDocument vehicleOwnersDocument = JsonDocument.Parse(vehicleOwnersResource);
                foreach (var vehicleOwnerElement in vehicleOwnersDocument.RootElement.EnumerateArray())
                {
                    // Get data from json
                    VehicleOwnerData vehicleOwnerData = new VehicleOwnerData();
                    vehicleOwnerData.Id = vehicleOwnerElement.TryGetProperty(nameof(vehicleOwnerData.Id), out var vehicleOwnerIdValue) 
                        ? vehicleOwnerIdValue.ToString() 
                        : default;
                    vehicleOwnerData.Name = (vehicleOwnerElement.TryGetProperty(nameof(vehicleOwnerData.Name), out var vehicleOwnerNameValue) 
                        ? vehicleOwnerNameValue.ToString() 
                        : default)!;
                    vehicleOwnerData.Address = (vehicleOwnerElement.TryGetProperty(nameof(vehicleOwnerData.Address), out var vehicleOwnerAddressValue) 
                        ? vehicleOwnerAddressValue.ToString() 
                        : default)!;
                
                    // Establish entity
                    VehicleOwner result = VehicleOwner.Create(
                        new Guid(vehicleOwnerData.Id),
                        vehicleOwnerData.Name,
                        vehicleOwnerData.Address,
                        new List<Vehicle>()
                    );
                    owners.Add(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
            
        // Seed the vehicles
        if (!context.Vehicles.Any())
        {
            string vehiclesResource = await File.ReadAllTextAsync(mainPath + _options.Vehicles);
            try
            {
                // Get data from json
                JsonDocument vehiclesDocument = JsonDocument.Parse(vehiclesResource);
                foreach (var vehicleElement in vehiclesDocument.RootElement.EnumerateArray())
                {
                    // Get data from json
                    VehicleData vehicleData = new VehicleData();
                    vehicleData.Id = vehicleElement.TryGetProperty(nameof(vehicleData.Id), out var vehicleIdValue) 
                        ? vehicleIdValue.ToString() 
                        : default;
                    vehicleData.Name = (vehicleElement.TryGetProperty(nameof(vehicleData.Name), out var vehicleNameValue) 
                        ? vehicleNameValue.ToString() 
                        : default)!;
                    vehicleData.Description = (vehicleElement.TryGetProperty(nameof(vehicleData.Description), out var vehicleDescriptionValue) 
                        ? vehicleDescriptionValue.ToString() 
                        : default)!;
                    vehicleData.Price = (vehicleElement.TryGetProperty(nameof(vehicleData.Price), out var vehiclePriceValue) 
                        ? decimal.Parse(vehiclePriceValue.ToString())
                        : default);
                    vehicleData.VehicleBrandId = (vehicleElement.TryGetProperty(nameof(vehicleData.VehicleBrandId), out var vehicleBrandIdValue) 
                        ? vehicleBrandIdValue.ToString() 
                        : default)!;
                    vehicleData.VehicleOwnerId = (vehicleElement.TryGetProperty(nameof(vehicleData.VehicleOwnerId), out var vehicleOwnerIdValue) 
                        ? vehicleOwnerIdValue.ToString() 
                        : default)!;
                
                    // Establish entity
                    Vehicle result = Vehicle.Create(
                        new Guid(vehicleData.Id),
                        vehicleData.Name,
                        vehicleData.Description,
                        vehicleData.Price,
                        owners.FirstOrDefault(owner => owner.Id.ToString() == vehicleData.VehicleOwnerId)!,
                        new Guid(vehicleData.VehicleOwnerId),
                        brands.FirstOrDefault(brand => brand.Id.ToString() == vehicleData.VehicleBrandId)!,
                        new Guid(vehicleData.VehicleBrandId)
                    );
                    vehicles.Add(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Seed the roles
        if (!context.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.Admin));
            await roleManager.CreateAsync(new IdentityRole(RoleConstants.User));
        }
        
        // Seed the users
        if (!context.Users.Any())
        {
            string usersResource = await File.ReadAllTextAsync(mainPath + _options.Users);
            try
            {
                // Get data from json
                JsonDocument usersDocument = JsonDocument.Parse(usersResource);
                foreach (var usersElement in usersDocument.RootElement.EnumerateArray())
                {
                    // Get data from json
                    UserData userData = new UserData();
                    userData.Name = (usersElement.TryGetProperty(nameof(userData.Name), out var userNameValue) 
                        ? userNameValue.ToString() 
                        : default)!;
                    userData.Email = (usersElement.TryGetProperty(nameof(userData.Name), out var userEmailValue) 
                        ? userEmailValue.ToString() 
                        : default)!;
                    userData.Password = (usersElement.TryGetProperty(nameof(userData.Password), out var userPasswordValue) 
                        ? userPasswordValue.ToString() 
                        : default)!;
                
                    // Establish entity
                    ApplicationUser result = new ApplicationUser()
                    {
                        Name = userData.Name,
                        UserName = userData.Email,
                        Email = userData.Email,
                        PasswordHash = userData.Password
                    };
                    await userManager.CreateAsync(result, userData.Password);
                    await userManager.AddToRoleAsync(result, RoleConstants.User);
                }
                
                // Establish admin
                ApplicationUser admin = new ApplicationUser()
                {
                    Name = AdminConstants.Name,
                    UserName = AdminConstants.Email,
                    Email = AdminConstants.Email,
                    PasswordHash = AdminConstants.Password
                };
                await userManager.CreateAsync(admin, AdminConstants.Password);
                await userManager.AddToRoleAsync(admin, RoleConstants.Admin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Add data
        context.VehicleBrands.AddRange(brands);
        context.VehicleOwners.AddRange(owners);
        context.Vehicles.AddRange(vehicles);
        
        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
        }
    }
}