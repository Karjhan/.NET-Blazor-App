namespace Infrastructure.Configurations;

public class SeedDataConfiguration
{
    public string BaseDirectory { get; init; } = null!;

    public string Owners { get; init; } = null!;
    
    public string Brands { get; init; } = null!;
    
    public string Vehicles { get; init; } = null!;
    
    public string Users { get; init; } = null!;
}