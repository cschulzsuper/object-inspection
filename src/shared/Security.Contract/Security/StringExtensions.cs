using System;
using System.Text.Json;
using Super.Paula.Shared.JsonConversion;

namespace Super.Paula.Shared.Security;

public static class StringExtensions
{
    public static Token? ToToken(this string token)
        => string.IsNullOrWhiteSpace(token)
            ? null
            : JsonSerializer.Deserialize<Token>(
                    Convert.FromBase64String(token), CustomJsonSerializerOptions.Default);
}