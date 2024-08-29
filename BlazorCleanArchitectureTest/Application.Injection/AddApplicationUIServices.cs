using Application.Adapters;
using Application.Services;
using Application.Utilities;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace Application.Injection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationUIServices(this IServiceCollection services, IConfiguration configuration)
    {
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
            client.BaseAddress = new Uri(configuration.GetSection("BaseAddress").Value!);
        }).AddHttpMessageHandler<CustomHttpHandler>();

        return services;
    }   
}