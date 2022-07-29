
// Modified verison of the original .NET ProblemDetailsJsonConverter,
// with custome changes and changes from .NET ValidationProblemDetailsJsonConverter.

using Super.Paula.Application.Inventory.Requests;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Super.Paula.Application.Inventory.Converter
{
    public class BusinessObjectRequestConverter : JsonConverter<BusinessObjectRequest>
    {
        private static readonly JsonEncodedText UniqueName = JsonEncodedText.Encode("uniqueName");
        private static readonly JsonEncodedText DisplayName = JsonEncodedText.Encode("displayName");
        private static readonly JsonEncodedText Inspector = JsonEncodedText.Encode("inspector");
        private static readonly JsonEncodedText ETag = JsonEncodedText.Encode("etag");

        public override BusinessObjectRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var problemDetails = new BusinessObjectRequest();

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Unexcepted end when reading JSON.");
            }

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                ReadValue(ref reader, problemDetails, options);
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException("Unexcepted end when reading JSON.");
            }

            return problemDetails;
        }

        public override void Write(Utf8JsonWriter writer, BusinessObjectRequest value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteBusinessObjectRequest(writer, value, options);
            writer.WriteEndObject();
        }

        internal static void ReadValue(ref Utf8JsonReader reader, BusinessObjectRequest value, JsonSerializerOptions options)
        {
            if (TryReadStringProperty(ref reader, Inspector, out var stringValue))
            {
                value.Inspector = stringValue;
            }
            else if (TryReadStringProperty(ref reader, DisplayName, out stringValue))
            {
                value.DisplayName = stringValue;
            }
            else if (TryReadStringProperty(ref reader, UniqueName, out stringValue))
            {
                value.UniqueName = stringValue;
            }
            else if (TryReadStringProperty(ref reader, ETag, out stringValue))
            {
                value.ETag = stringValue;
            }
            else
            {
                var key = reader.GetString()!;
                reader.Read();

                var objectConvertOptions = new JsonSerializerOptions(options);
                objectConvertOptions.Converters.Add(new ObjectJsonConverter());

                var convertedObject = JsonSerializer.Deserialize(ref reader, typeof(object), objectConvertOptions);

                if (convertedObject != null)
                {
                    value[key] = convertedObject;
                }
            }
        }

        internal static bool TryReadStringProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, [NotNullWhen(true)] out string? value)
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

        internal static bool TryReadBoolProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, [NotNullWhen(true)] out bool value)
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

        internal static void WriteBusinessObjectRequest(Utf8JsonWriter writer, BusinessObjectRequest value, JsonSerializerOptions options)
        {
            if (value.Inspector != null)
            {
                writer.WriteString(Inspector, value.Inspector);
            }

            if (value.UniqueName != null)
            {
                writer.WriteString(UniqueName, value.UniqueName);
            }

            if (value.DisplayName != null)
            {
                writer.WriteString(DisplayName, value.DisplayName);
            }

            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key);
                JsonSerializer.Serialize(writer, kvp.Value, kvp.Value?.GetType() ?? typeof(object), options);
            }
        }
    }
}