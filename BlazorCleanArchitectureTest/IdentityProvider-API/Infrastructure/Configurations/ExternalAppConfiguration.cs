namespace IdentityProvider_API.Infrastructure.Configurations;

public class ExternalAppConfiguration
{
    public string BaseURL { get; set; } = string.Empty;

    public string RedirectPath { get; set; } = string.Empty;
    
    public string ClientId { get; set; } = string.Empty;
    
    public string ClientSecret { get; set; } = string.Empty;
}