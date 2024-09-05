using Domain.Models.Authentication;
using Infrastructure.DataContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.SeedData.Initializer;

public interface IDataContextSeed
{
    Task SeedAsync(ApplicationContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IHostEnvironment environment);
}