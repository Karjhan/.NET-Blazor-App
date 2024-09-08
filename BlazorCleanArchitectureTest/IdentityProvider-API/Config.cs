using Duende.IdentityServer.Models;
using IdentityProvider_API.Infrastructure.Configurations;

namespace IdentityProvider_API;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("blazorApp", "Blazor Test App Full Access"),
        };

    public static IEnumerable<Client> Clients (ExternalAppConfiguration externalApp) =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = {"openid", "profile", "blazorApp"},
                RedirectUris = {"https://www.getpostman.com/oauth2/callback"},
                ClientSecrets = new [] {new Secret("NotASecret".Sha256())},
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword}
            },
            new Client
            {
                ClientId = "backendAPI",
                ClientName = "BackendAPI",
                AllowedScopes = {"openid", "profile", "blazorApp"},
                RedirectUris = {$"{externalApp.BaseURL}{externalApp.RedirectPath}"},
                ClientSecrets = new [] {new Secret("BackendAPISuperSecret".Sha256())},
                AllowedGrantTypes = {GrantType.AuthorizationCode, GrantType.ResourceOwnerPassword}
            },
        };
}