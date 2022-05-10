
// Modified verison of the original .NET ProblemDetailsJsonConverter,
// with custome changes and changes from .NET ValidationProblemDetailsJsonConverter.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Super.Paula.ErrorHandling
{
    public class ClientProblemDetailsJsonConverter : JsonConverter<ClientProblemDetails>
    {
        private static readonly JsonEncodedText Type = JsonEncodedText.Encode("type");
        private static readonly JsonEncodedText Title = JsonEncodedText.Encode("title");
        private static readonly JsonEncodedText Status = JsonEncodedText.Encode("status");
        private static readonly JsonEncodedText Detail = JsonEncodedText.Encode("detail");
        private static readonly JsonEncodedText Instance = JsonEncodedText.Encode("instance");

        private static readonly JsonEncodedText Errors = JsonEncodedText.Encode("errors");
        private static readonly JsonEncodedText TitleFormat = JsonEncodedText.Encode("titleFormat");
        private static readonly JsonEncodedText TitleArguments = JsonEncodedText.Encode("titleArguments");

        public override ClientProblemDetails Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var problemDetails = new ClientProblemDetails();

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

        public override void Write(Utf8JsonWriter writer, ClientProblemDetails value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteProblemDetails(writer, value, options);
            writer.WriteEndObject();
        }

        internal static void ReadValue(ref Utf8JsonReader reader, ClientProblemDetails value, JsonSerializerOptions options)
        {
            if (TryReadStringProperty(ref reader, Type, out var propertyValue))
            {
                value.Type = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Title, out propertyValue))
            {
                value.Title = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Detail, out propertyValue))
            {
                value.Detail = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, Instance, out propertyValue))
            {
                value.Instance = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, TitleFormat, out propertyValue))
            {
                value.Extensions["titleFormat"] = propertyValue;
            }
            else if (reader.ValueTextEquals(TitleArguments.EncodedUtf8Bytes))
            {
                value.Extensions["titleArguments"] = DeserializeTitleArguments(ref reader, options);
            }
            else if (reader.ValueTextEquals(Errors.EncodedUtf8Bytes))
            {
                value.Extensions["errors"] = DeserializeErrors(ref reader, options);
            }
            else if (reader.ValueTextEquals(Status.EncodedUtf8Bytes))
            {
                reader.Read();
                if (reader.TokenType == JsonTokenType.Null)
                {
                    // Nothing to do here.
                }
                else
                {
                    value.Status = reader.GetInt32();
                }
            }
            else
            {
                var key = reader.GetString()!;
                reader.Read();
                value.Extensions[key] = JsonSerializer.Deserialize(ref reader, typeof(object), options);
            }

            static Dictionary<string, string[]>? DeserializeErrors(ref Utf8JsonReader reader, JsonSerializerOptions options)
                => JsonSerializer.Deserialize<Dictionary<string, string[]>>(ref reader, options);

            static object[]? DeserializeTitleArguments(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                var objectConvertOptions = new JsonSerializerOptions(options);
                objectConvertOptions.Converters.Add(new ObjectJsonConverter());
                return JsonSerializer.Deserialize<object[]>(ref reader, objectConvertOptions);
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

        internal static void WriteProblemDetails(Utf8JsonWriter writer, ClientProblemDetails value, JsonSerializerOptions options)
        {
            if (value.Type != null)
            {
                writer.WriteString(Type, value.Type);
            }

            if (value.Title != null)
            {
                writer.WriteString(Title, value.Title);
            }

            if (value.Status != null)
            {
                writer.WriteNumber(Status, value.Status.Value);
            }

            if (value.Detail != null)
            {
                writer.WriteString(Detail, value.Detail);
            }

            if (value.Instance != null)
            {
                writer.WriteString(Instance, value.Instance);
            }

            foreach (var kvp in value.Extensions)
            {
                writer.WritePropertyName(kvp.Key);
                JsonSerializer.Serialize(writer, kvp.Value, kvp.Value?.GetType() ?? typeof(object), options);
            }
        }
    }
}