using System.Text.Json.Serialization;

namespace Infrastructure.DTOs;

public class VehicleBrandData
{
    [JsonPropertyName("Id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("Location")]
    public string Location { get; set; } = string.Empty;
}