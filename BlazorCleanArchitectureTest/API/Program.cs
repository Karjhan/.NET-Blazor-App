using API.Extensions;
using API.Middlewares;
using Application;
using Application.Extensions;
using Carter;
using Domain.Models.Authentication;
using Infrastructure.DataContexts;
using Infrastructure.Extensions;
using Infrastructure.SeedData.Initializer;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add logging with Serilog
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddSwaggerDocumentation();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationAPIServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration);

builder.Services.AddSwaggerDocumentation();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Configuration.GetValue("EnforceHttpsRedirection", true))
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<RequestLogContextMiddleware>();

app.UseSerilogRequestLogging();

app.MapApplicationHealthChecks();

app.UseSwaggerDocumentation();

app.MapCarter();

// Auto applying new migrations at build+run, or creating the DB otherwise + initial seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationContext>();
    var dataContextSeed = services.GetRequiredService<IDataContextSeed>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var env = services.GetRequiredService<IHostEnvironment>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Starting Context Initializer.");
        await context.Database.MigrateAsync();
        await dataContextSeed.SeedAsync(context, roleManager, userManager, env);
    }
    catch (Exception e)
    {
        logger.LogError("An error occured during migration, or seed!");
    }
}

app.Run();