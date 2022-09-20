using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChristianSchulz.ObjectInspection.Shared.JsonConversion;

namespace ChristianSchulz.ObjectInspection.Shared.Security;

public class ClaimsJsonConverter : JsonConverter<Claim[]>
{
    private static readonly JsonEncodedText Identity = JsonEncodedText.Encode("identity");
    private static readonly JsonEncodedText Proof = JsonEncodedText.Encode("proof");
    private static readonly JsonEncodedText Inspector = JsonEncodedText.Encode("inspector");
    private static readonly JsonEncodedText Organization = JsonEncodedText.Encode("organization");
    private static readonly JsonEncodedText ImpersonatorOrganization = JsonEncodedText.Encode("impersonatorOrganization");
    private static readonly JsonEncodedText ImpersonatorInspector = JsonEncodedText.Encode("impersonatorInspector");
    private static readonly JsonEncodedText Authorization = JsonEncodedText.Encode("authorization");

    public override Claim[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var request = new List<Claim>();

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Unexpected end when reading JSON.");
        }

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Unexpected end when reading JSON.");
            }

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                ReadValue(ref reader, request, options);
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException("Unexpected end when reading JSON.");
            }
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Unexpected end when reading JSON.");
        }

        return request.ToArray();
    }

    internal static void ReadValue(ref Utf8JsonReader reader, ICollection<Claim> value, JsonSerializerOptions options)
    {
        if (reader.TryReadStringProperty(Identity, out var stringValue))
        {
            value.Add(new Claim(nameof(Identity),stringValue));
            return;
        }

        if (reader.TryReadStringProperty(Proof, out stringValue))
        {
            value.Add(new Claim(nameof(Proof), stringValue));
            return;
        }

        if (reader.TryReadStringProperty(Inspector, out stringValue))
        {
            value.Add(new Claim(nameof(Inspector), stringValue));
            return;
        }

        if (reader.TryReadStringProperty(Organization, out stringValue))
        {
            value.Add(new Claim(nameof(Organization), stringValue));
            return;
        }

        if (reader.TryReadStringProperty(ImpersonatorOrganization, out stringValue))
        {
            value.Add(new Claim(nameof(ImpersonatorOrganization), stringValue));
            return;
        }

        if (reader.TryReadStringProperty(ImpersonatorInspector, out stringValue))
        {
            value.Add(new Claim(nameof(ImpersonatorInspector), stringValue));
            return;
        }

        if (reader.TryReadStringProperty(Authorization, out stringValue))
        {
            value.Add(new Claim(nameof(Authorization), stringValue));
            return;
        }
    }

    public override void Write(Utf8JsonWriter writer, Claim[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var claim in value)
        {
            var propertyName = GetPropertyNameOrDefault(claim.Type);
            var propertyValue = claim.Value;

            if (propertyName != null)
            {
                writer.WriteStartObject();
                writer.WriteStringIfNotNull(propertyName.Value, propertyValue);
                writer.WriteEndObject();
            }
        }

        writer.WriteEndArray();
    }

    private JsonEncodedText? GetPropertyNameOrDefault(string claimType)
        => claimType switch
        {
            nameof(Identity) => Identity,
            nameof(Proof) => Proof,
            nameof(Inspector) => Inspector,
            nameof(Organization) => Organization,
            nameof(ImpersonatorOrganization) => ImpersonatorOrganization,
            nameof(ImpersonatorInspector) => ImpersonatorInspector,
            nameof(Authorization) => Authorization,
            _ => null
        };
}
