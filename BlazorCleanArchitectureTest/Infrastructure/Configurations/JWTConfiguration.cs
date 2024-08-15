namespace Infrastructure.Configurations;

public class JWTConfiguration
{
    public string Key { get; init; } = null!;
    
    public string Issuer { get; init; } = null!;
    
    public string Audience { get; init; } = null!;
}