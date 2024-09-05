using System.Text.Json.Serialization;

namespace Infrastructure.DTOs;

public class VehicleData
{
    [JsonPropertyName("Id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("Description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("Price")]
    public decimal Price { get; set; } 
    
    [JsonPropertyName("VehicleOwnerId")]
    public string VehicleOwnerId { get; set; } = string.Empty;
    
    [JsonPropertyName("VehicleBrandId")]
    public string VehicleBrandId { get; set; } = string.Empty;
}