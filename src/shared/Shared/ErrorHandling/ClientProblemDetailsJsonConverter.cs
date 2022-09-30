
// Modified verison of the original .NET ProblemDetailsJsonConverter,
// with custome changes and changes from .NET ValidationProblemDetailsJsonConverter.

using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChristianSchulz.ObjectInspection.Shared.ErrorHandling;

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
            throw new JsonException("Unexpected end when reading JSON.");
        }

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            ReadValue(ref reader, problemDetails, options);
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Unexpected end when reading JSON.");
        }

        return problemDetails;
    }

    private static void ReadValue(ref Utf8JsonReader reader, ClientProblemDetails value, JsonSerializerOptions options)
    {
        if (reader.TryReadStringProperty(Type, out var propertyValue))
        {
            value.Type = propertyValue;
            return;
        }

        if (reader.TryReadStringProperty(Title, out propertyValue))
        {
            value.Title = propertyValue;
            return;
        }

        if (reader.TryReadStringProperty(Detail, out propertyValue))
        {
            value.Detail = propertyValue;
            return;
        }

        if (reader.TryReadStringProperty(Instance, out propertyValue))
        {
            value.Instance = propertyValue;
            return;
        }

        if (reader.TryReadStringProperty(TitleFormat, out propertyValue))
        {
            value.Extensions["titleFormat"] = propertyValue;
            return;
        }

        if (reader.ValueTextEquals(TitleArguments.EncodedUtf8Bytes))
        {
            value.Extensions["titleArguments"] = JsonSerializer.Deserialize<object[]>(ref reader, options); ;
            return;
        }

        if (reader.ValueTextEquals(Errors.EncodedUtf8Bytes))
        {
            value.Extensions["errors"] = JsonSerializer.Deserialize<Dictionary<string, string[]>>(ref reader, options);
            return;
        }

        if (reader.ValueTextEquals(Status.EncodedUtf8Bytes))
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.Null)
            {
                value.Status = reader.GetInt32();
            }
            return;
        }

        var key = reader.GetString()!;
        reader.Read();
        if (reader.TokenType != JsonTokenType.Null)
        {
            value.Extensions[key] = JsonSerializer.Deserialize(ref reader, typeof(object), options);
        }
    }

    public override void Write(Utf8JsonWriter writer, ClientProblemDetails value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteStringIfNotNull(Type, value.Type);
        writer.WriteStringIfNotNull(Title, value.Title);
        writer.WriteNumberIfNotNull(Status, value.Status);
        writer.WriteStringIfNotNull(Detail, value.Detail);
        writer.WriteStringIfNotNull(Instance, value.Instance);

        foreach (var extension in value.Extensions)
        {
            writer.WriteObjectIfNotNull(extension.Key, extension.Value, options);
        }

        writer.WriteEndObject();
    }
}