using System.Text.Json;

namespace Super.Paula.JsonConversion
{
    public class CustomJsonSerializerOptions
    {
        public readonly static JsonSerializerOptions Default = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new CustomJsonCamelCaseNamingPolicy(),
            Converters = { new ObjectJsonConverter() }
        };

        public readonly static JsonSerializerOptions WebResponse = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = new CustomJsonCamelCaseNamingPolicy(),
            Converters = { new ObjectJsonConverter() },
            DefaultBufferSize = 128
        };
    }
}
