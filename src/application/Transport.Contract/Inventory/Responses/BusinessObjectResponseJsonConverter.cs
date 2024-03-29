﻿using ChristianSchulz.ObjectInspection.Shared.JsonConversion;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChristianSchulz.ObjectInspection.Application.Inventory.Responses;

public class BusinessObjectResponseJsonConverter : JsonConverter<BusinessObjectResponse>
{
    private static readonly JsonEncodedText UniqueName = JsonEncodedText.Encode("uniqueName");
    private static readonly JsonEncodedText DisplayName = JsonEncodedText.Encode("displayName");
    private static readonly JsonEncodedText DistinctionType = JsonEncodedText.Encode("distinctionType");
    private static readonly JsonEncodedText ETag = JsonEncodedText.Encode("etag");

    public override BusinessObjectResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var response = new BusinessObjectResponse();

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Unexpected end when reading JSON.");
        }

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            ReadValue(ref reader, response, options);
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Unexpected end when reading JSON.");
        }

        return response;
    }

    internal static void ReadValue(ref Utf8JsonReader reader, BusinessObjectResponse value, JsonSerializerOptions options)
    {
        if (reader.TryReadStringProperty(DisplayName, out var stringValue))
        {
            value.DisplayName = stringValue;
            return;
        }

        if (reader.TryReadStringProperty(ETag, out stringValue))
        {
            value.ETag = stringValue;
            return;
        }

        if (reader.TryReadStringProperty(UniqueName, out stringValue))
        {
            value.UniqueName = stringValue;
            return;
        }

        if (reader.TryReadStringProperty(DistinctionType, out stringValue))
        {
            value.DistinctionType = stringValue;
            return;
        }

        if (reader.TryReadExtensionProperty(out var extensionKey, out var extensionValue, options))
        {
            value[extensionKey] = extensionValue;
            return;
        }
    }

    public override void Write(Utf8JsonWriter writer, BusinessObjectResponse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteStringIfNotNull(UniqueName, value.UniqueName);
        writer.WriteStringIfNotNull(DisplayName, value.DisplayName);
        writer.WriteStringIfNotNull(DistinctionType, value.DistinctionType);
        writer.WriteStringIfNotNull(ETag, value.ETag);

        foreach (var extension in value)
        {
            writer.WriteObjectIfNotNull(extension.Key, extension.Value, options);
        }

        writer.WriteEndObject();
    }

}