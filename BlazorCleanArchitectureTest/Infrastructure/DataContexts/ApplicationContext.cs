using System.Reflection;
using Domain.Models.Authentication;
using Infrastructure.Abstractions.Persistence;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataContexts;

public class ApplicationContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    } 
}