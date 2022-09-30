using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.Shared.Security;

public static class ClaimsExtensions
{
    public static string GetIdentity(this IEnumerable<Claim> claims)
        => claims.First(x => x.Type == "Identity").Value;

    public static string GetProof(this IEnumerable<Claim> claims)
        => claims.First(x => x.Type == "Proof").Value;

    public static string GetOrganization(this IEnumerable<Claim> claims)
        => claims.First(x => x.Type == "Organization").Value;

    public static string GetInspector(this IEnumerable<Claim> claims)
        => claims.First(x => x.Type == "Inspector").Value;

    public static string GetImpersonatorOrganization(this IEnumerable<Claim> claims)
        => claims.First(x => x.Type == "ImpersonatorOrganization").Value;

    public static string GetImpersonatorInspector(this IEnumerable<Claim> claims)
        => claims.First(x => x.Type == "ImpersonatorInspector").Value;

    public static string[] GetAuthorizations(this IEnumerable<Claim> claims)
        => claims
            .Where(x => x.Type == "Authorization")
            .Select(x => x.Value)
            .ToArray();

    public static bool HasIdentity(this IEnumerable<Claim> claims)
        => claims.Any(x =>
            x.Type == "Identity" &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasIdentity(this IEnumerable<Claim> claims, string identity)
        => claims.Any(x =>
            x.Type == "Identity" &&
            x.Value == identity &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasOrganization(this IEnumerable<Claim> claims)
        => claims.Any(x =>
            x.Type == "Organization" &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasOrganization(this IEnumerable<Claim> claims, string organization)
        => claims.Any(x =>
            x.Type == "Organization" &&
            x.Value == organization &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasInspector(this IEnumerable<Claim> claims)
        => claims.Any(x =>
            x.Type == "Inspector" &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasInspector(this IEnumerable<Claim> claims, string inspector)
        => claims.Any(x =>
            x.Type == "Inspector" &&
            x.Value == inspector &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasProof(this IEnumerable<Claim> claims)
        => claims.Any(x =>
            x.Type == "Proof" &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasProof(this IEnumerable<Claim> claims, string proof)
        => claims.Any(x =>
            x.Type == "Proof" &&
            x.Value == proof &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasImpersonatorOrganization(this IEnumerable<Claim> claims)
        => claims.Any(x =>
            x.Type == "ImpersonatorOrganization" &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasImpersonatorOrganization(this IEnumerable<Claim> claims, string organization)
        => claims.Any(x =>
            x.Type == "ImpersonatorOrganization" &&
            x.Value == organization &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasImpersonatorInspector(this IEnumerable<Claim> claims)
        => claims.Any(x =>
            x.Type == "ImpersonatorInspector" &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasImpersonatorInspector(this IEnumerable<Claim> claims, string inspector)
        => claims.Any(x =>
            x.Type == "ImpersonatorInspector" &&
            x.Value == inspector &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasAuthorization(this IEnumerable<Claim> claims, string authorization)
        => claims.Any(x =>
            x.Type == "Authorization" &&
            x.Value == authorization &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasAnyAuthorization(this IEnumerable<Claim> claims, IEnumerable<string> authorizations)
        => claims.Any(x =>
            x.Type == "Authorization" &&
            authorizations.Contains(x.Value) &&
            !string.IsNullOrWhiteSpace(x.Value));

    public static bool HasAnyAuthorization(this IEnumerable<Claim> claims, params string[] authorizations)
        => claims.HasAnyAuthorization(authorizations.AsEnumerable());
}