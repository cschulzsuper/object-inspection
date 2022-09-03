using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Super.Paula.Shared.JsonConversion;

namespace Super.Paula.Shared.Security;

public static class BadgeExtensions
{
    public static string ToBase64String(this Badge badge)
        => Convert.ToBase64String(
                JsonSerializer.SerializeToUtf8Bytes(badge, CustomJsonSerializerOptions.Default));

    public static Claim[] ToClaims(this Badge badge)
    {
        var claims = Enumerable.Empty<Claim>()
            .Concat(GetSimplePropertyClaims(badge))
            .Concat(GetArrayPropertyClaims(badge))
            .ToList();

        if (!string.IsNullOrWhiteSpace(badge.Organization) &&
            !string.IsNullOrWhiteSpace(badge.Inspector))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, $"{badge.Organization}:{badge.Inspector}"));
        }

        return claims.ToArray();
    }

    private static IEnumerable<Claim> GetSimplePropertyClaims(Badge badge)
         => badge
            .GetType()
            .GetProperties()
            .Where(x => !x.PropertyType.IsAssignableTo(typeof(IEnumerable<string>)))
            .Select(x => (x.Name, x.GetValue(badge)))
            .Where(x => x.Item2 != null)
            .Select(x => new Claim(x.Name, $"{x.Item2}"));

    private static IEnumerable<Claim> GetArrayPropertyClaims(Badge badge)
         => badge
            .GetType()
            .GetProperties()
            .Where(x => x.PropertyType.IsAssignableTo(typeof(IEnumerable<string>)))
            .Select(x => (
                MakeSingular(x.Name),
                (IEnumerable<string>?)x.GetValue(badge)))
            .Where(x => x.Item2 != null)
            .SelectMany(x => x.Item2!.Select(y => new Claim(x.Item1, $"{y}")));

    private static string MakeSingular(string plural)
        => plural switch
        {
            nameof(Badge.Authorizations) => "Authorization",
            _ => plural
        };
}