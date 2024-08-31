using System.Reflection;
using Application.Adapters;
using Application.Services;
using Application.Utilities;
using Infrastructure.Constants;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationAPIServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

            config.NotificationPublisher = new TaskWhenAllPublisher();
        });
        
        // Add AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        return services;
    }  
    
    public static IServiceCollection AddApplicationUIServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services
        services.AddScoped<IAccountService, AccountService>();

        services.AddAuthorizationCore();
        services.AddNetcodeHubLocalStorageService();

        services.AddScoped<ILocalStorageAdapter, LocalStorageAdapter>();
        services.AddScoped<IBackendApiAdapter, BackendApiAdapter>();

        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddTransient<CustomHttpHandler>();

        services.AddCascadingAuthenticationState();
        services.AddHttpClient(ApplicationConstants.BackendApiClientName, client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("BaseAPIAddress").Value ?? "https://localhost:6443/");
        }).AddHttpMessageHandler<CustomHttpHandler>();
        
        return services;
    }  
}