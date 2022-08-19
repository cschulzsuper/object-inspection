using System.Text.Json;

namespace Super.Paula.JsonConversion
{
    public static class Utf8JsonWriterExtensions
    {
        public static void WriteStringIfNotNull(this Utf8JsonWriter writer, JsonEncodedText propertyName, string? value)
        {
            if (value != null)
            {
                writer.WriteString(propertyName, value);
            }
        }

        public static void WriteNumberIfNotNull(this Utf8JsonWriter writer, JsonEncodedText propertyName, int? value)
        {
            if (value != null)
            {
                writer.WriteNumber(propertyName, value.Value);
            }
        }

        public static void WriteObjectIfNotNull(this Utf8JsonWriter writer, string propertyName, object? value, JsonSerializerOptions options)
        {
            if (value != null)
            {
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value, value.GetType() ?? typeof(object), options);
            }
        }
    }
}
