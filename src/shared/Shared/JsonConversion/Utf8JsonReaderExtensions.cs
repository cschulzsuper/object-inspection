using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Super.Paula.Shared.JsonConversion;

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
        extensionValue = JsonSerializer.Deserialize(ref reader, typeof(object), options)!;
        return extensionValue != null;
    }
}