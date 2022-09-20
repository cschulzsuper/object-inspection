using System.Text.Json;

namespace ChristianSchulz.ObjectInspection.Shared.JsonConversion;

public class CustomJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Default = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = new CustomJsonCamelCaseNamingPolicy(),
        Converters = { /* new ObjectJsonConverter() */ }
    };

    public static readonly JsonSerializerOptions WebResponse = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = new CustomJsonCamelCaseNamingPolicy(),
        Converters = { /* new ObjectJsonConverter() */ },
        DefaultBufferSize = 128
    };
}