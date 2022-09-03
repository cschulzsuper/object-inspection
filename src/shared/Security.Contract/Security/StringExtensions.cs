using System;
using System.Text.Json;
using Super.Paula.Shared.JsonConversion;

namespace Super.Paula.Shared.Security;

public static class StringExtensions
{
    public static Badge? ToBadge(this string badge)
        => string.IsNullOrWhiteSpace(badge)
            ? null
            : JsonSerializer.Deserialize<Badge>(
                    Convert.FromBase64String(badge), CustomJsonSerializerOptions.Default);
}