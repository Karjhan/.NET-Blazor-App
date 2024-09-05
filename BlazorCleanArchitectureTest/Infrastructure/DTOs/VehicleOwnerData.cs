using System.Text.Json.Serialization;

namespace Infrastructure.DTOs;

public class VehicleOwnerData
{
    [JsonPropertyName("Id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("Address")]
    public string Address { get; set; } = string.Empty;
}