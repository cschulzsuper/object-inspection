using Super.Paula.JsonConversion;
using System;
using System.Text.Json;

namespace Super.Paula.Authorization
{
    public static class StringExtensions
    {
        public static Token? ToToken(this string token)
            => string.IsNullOrWhiteSpace(token)
                ? null
                : JsonSerializer.Deserialize<Token>(
                        Convert.FromBase64String(token), CustomJsonSerializerOptions.Default);
    }
}
