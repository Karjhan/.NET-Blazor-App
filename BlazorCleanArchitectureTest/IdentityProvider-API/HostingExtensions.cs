using Duende.IdentityServer;
using HealthChecks.UI.Client;
using IdentityProvider_API.Infrastructure.Configurations;
using IdentityProvider_API.Infrastructure.DataContexts;
using IdentityProvider_API.Infrastructure.HealthChecks;
using IdentityProvider_API.Models;
using IdentityProvider_API.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityProvider_API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddRazorPages();

        builder.Services.Configure<EFConfiguration>(configuration.GetSection(nameof(EFConfiguration)).Bind);
        var efConfiguration = configuration.GetSection(nameof(EFConfiguration)).Get<EFConfiguration>();
        
        builder.Services.Configure<ExternalAppConfiguration>(configuration.GetSection(nameof(ExternalAppConfiguration)).Bind);
        var externalAppConfiguration = configuration.GetSection(nameof(ExternalAppConfiguration)).Get<ExternalAppConfiguration>();
        
        // Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck<ApplicationHealthCheck>("IdentityProvider-API")
            .AddNpgSql(efConfiguration!.ConnectionString);
        
        builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(efConfiguration!.ConnectionString));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                var dockerizedUrl = configuration.GetSection("DockerizedUrl").Value;
                if (!string.IsNullOrEmpty(dockerizedUrl))
                {
                    options.IssuerUri = dockerizedUrl;
                }
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients(externalAppConfiguration!))
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
        });

        builder.Services.AddAuthentication();
        
        // Add CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DefaultCorsPolicy", policy =>
            {
                var origins = configuration.GetSection("AllowedOrigins").Get<string[]>();
                if (origins?.Length > 0)
                {
                    policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
                else
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors("DefaultCorsPolicy");
        
        app.MapApplicationHealthChecks();
        
        app.UseSerilogRequestLogging();
        
        if (app.Configuration.GetValue("EnforceHttpsRedirection", true))
        {
            app.UseHttpsRedirection();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
    
    private static IEndpointConventionBuilder MapApplicationHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        // All
        var result = endpoints.MapHealthChecks("/healthCheck", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return result;
    }  
}