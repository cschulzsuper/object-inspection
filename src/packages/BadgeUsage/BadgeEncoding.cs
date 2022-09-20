using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace ChristianSchulz.ObjectInspection.BadgeUsage;

public class BadgeEncoding : IBadgeEncoding
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public BadgeEncoding(JsonSerializerOptions jsonSerializerOptions)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public ICollection<Claim> Decode(string badge)
    {
        var badgeClaims = string.IsNullOrWhiteSpace(badge)
            ? Array.Empty<Claim>()
            : JsonSerializer.Deserialize<Claim[]>(
                Convert.FromBase64String(badge), _jsonSerializerOptions);

        return (badgeClaims ?? Array.Empty<Claim>()).ToList();
    }

    public string Encode(IEnumerable<Claim> badgeClaims)
    {
        var badgeClaimList = badgeClaims as Claim[] ?? badgeClaims.ToArray();

        return Convert.ToBase64String(
            JsonSerializer.SerializeToUtf8Bytes(badgeClaimList, _jsonSerializerOptions));
    }
}
