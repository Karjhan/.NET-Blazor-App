using System.Text;
using Domain.Models.Authentication;
using Infrastructure.Configurations;
using Infrastructure.DataContexts;
using Infrastructure.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Swagger 
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        // Add EF Core configuration
        services.Configure<EFConfiguration>(configuration.GetSection(nameof(EFConfiguration)).Bind);

        var efConfiguration = configuration.GetSection(nameof(EFConfiguration)).Get<EFConfiguration>();
        
        // Add health checks
        services.AddHealthChecks()
            .AddCheck<ApplicationHealthCheck>("Product-Service-App")
            .AddNpgSql(efConfiguration!.ConnectionString);
        
        // Add entity dbContext for app, add postgres connection for dbContext
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(efConfiguration.ConnectionString);
        });
        
        // Add JWT configuration
        services.Configure<JWTConfiguration>(configuration.GetSection(nameof(JWTConfiguration)).Bind);

        var jwtConfiguration = configuration.GetSection(nameof(JWTConfiguration)).Get<JWTConfiguration>();
        
        // Add Identity 
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddSignInManager();
        
        // Add in-app authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtConfiguration!.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key))
            };
            options.RequireHttpsMetadata = true;
        });
        
        // Add OpenID Connect authentication (ex: Duende Identity Server)
        if (!string.IsNullOrEmpty(jwtConfiguration.DefaultThirdPartyUrl))
        {
            services.AddAuthentication()
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = jwtConfiguration.DefaultThirdPartyUrl;
                    options.ClientId = "your-client-id";
                    options.ClientSecret = "your-client-secret";
                    options.ResponseType = "code";

                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("api");

                    options.SaveTokens = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://your-duende-identity-server",
                        ValidateAudience = true,
                        ValidAudience = "your-audience",
                        ValidateLifetime = true
                    };
                });
        }
        
        // Add authorization
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CombinedPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "oidc");
            });
        });
        
        // Add CORS
        services.AddCors(options =>
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
        
        return services;
    }
}