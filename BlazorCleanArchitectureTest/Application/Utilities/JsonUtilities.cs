using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Utilities;

public static class JsonUtilities
{
    public static string SerializeObject<T>(T modelObject)
    {
        return JsonSerializer.Serialize(modelObject, JsonOptions());
    }

    public static T DeserializeJsonString<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
    }

    private static JsonSerializerOptions JsonOptions()
    {
        return new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };
    }
}