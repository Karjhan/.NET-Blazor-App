using System.Text.Json.Serialization;

namespace Infrastructure.DTOs;

public class UserData
{
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("Email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("Password")]
    public string Password { get; set; } = string.Empty;
}