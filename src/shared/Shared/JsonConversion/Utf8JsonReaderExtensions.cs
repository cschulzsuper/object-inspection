using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ChristianSchulz.ObjectInspection.Shared.JsonConversion;

public static class Utf8JsonReaderExtensions
{
    public static bool TryReadStringProperty(this ref Utf8JsonReader reader, JsonEncodedText propertyName, [NotNullWhen(true)] out string? value)
    {
        if (!reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
        {
            value = default;
            return false;
        }

        reader.Read();
        value = reader.GetString()!;
        return true;
    }

    public static bool TryReadBoolProperty(this ref Utf8JsonReader reader, JsonEncodedText propertyName, [NotNullWhen(true)] out bool value)
    {
        if (!reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
        {
            value = default;
            return false;
        }

        reader.Read();
        value = reader.GetBoolean()!;
        return true;
    }

    public static bool TryReadExtensionProperty(this ref Utf8JsonReader reader,
        [NotNullWhen(true)] out string extensionName,
        [NotNullWhen(true)] out object extensionValue,
        JsonSerializerOptions? options = null)
    {
        extensionName = reader.GetString()!;

        reader.Read();
        var deserializedValue = JsonSerializer.Deserialize(ref reader, typeof(object), options)!;

        extensionValue = deserializedValue switch
        {
            JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.True => true,
            JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.False => false,
            JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.Number && jsonElement.TryGetInt64(out long l) => l,
            JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.Number => jsonElement.GetDecimal(),
            JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.String => jsonElement.GetString()!,
            _ => null!
        };

        return extensionValue != null;
    }
}