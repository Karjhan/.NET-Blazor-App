using System.Text;
using Carter;
using Infrastructure.Configurations;
using Infrastructure.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class PresentationServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add EF Core configuration
        services.Configure<EFConfiguration>(configuration.GetSection(nameof(EFConfiguration)).Bind);

        var efConfiguration = configuration.GetSection(nameof(EFConfiguration)).Get<EFConfiguration>();
        
        // Add health checks
        services.AddHealthChecks()
            .AddCheck<ApplicationHealthCheck>("Backend-API")
            .AddNpgSql(efConfiguration!.ConnectionString);
        
        // Add JWT configuration
        services.Configure<JWTConfiguration>(configuration.GetSection(nameof(JWTConfiguration)).Bind);

        var jwtConfiguration = configuration.GetSection(nameof(JWTConfiguration)).Get<JWTConfiguration>();
        
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
        
        // Add Carter
        services.AddCarter();

        return services;
    }
}