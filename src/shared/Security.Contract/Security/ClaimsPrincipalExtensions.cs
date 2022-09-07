using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Super.Paula.Shared.Security;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticatedInspector(this ClaimsPrincipal principal)
        => principal.Identity?.IsAuthenticated == true &&
           principal.HasInspector();

    public static bool IsAuthenticatedIdentity(this ClaimsPrincipal principal)
        => principal.Identity?.IsAuthenticated == true &&
           principal.HasIdentity();

    public static string GetIdentity(this ClaimsPrincipal principal)
        => principal.FindAll("Identity").First().Value;

    public static string GetProof(this ClaimsPrincipal principal)
        => principal.FindAll("Proof").First().Value;

    public static string GetOrganization(this ClaimsPrincipal principal)
        => principal.FindAll("Organization").First().Value;

    public static string GetInspector(this ClaimsPrincipal principal)
        => principal.FindAll("Inspector").First().Value;

    public static string GetImpersonatorOrganization(this ClaimsPrincipal principal)
        => principal.FindAll("ImpersonatorOrganization").First().Value;

    public static string GetImpersonatorInspector(this ClaimsPrincipal principal)
        => principal.FindAll("ImpersonatorInspector").First().Value;

    public static string[] GetAuthorizations(this ClaimsPrincipal principal)
        => principal.FindAll("Authorization")
            .Where(x =>
                !principal.FindAll("AuthorizationFilter").Any() ||
                principal.HasClaim("AuthorizationFilter", x.Value))
            .Select(x => x.Value)
            .ToArray();

    public static bool HasIdentity(this ClaimsPrincipal principal)
        => principal.HasClaim(x => x.Type == "Identity");

    public static bool HasIdentity(this ClaimsPrincipal principal, string identity)
        => principal.HasClaim("Identity", identity);

    public static bool HasOrganization(this ClaimsPrincipal principal)
        => principal.HasClaim(x => x.Type == "Organization");

    public static bool HasOrganization(this ClaimsPrincipal principal, string organization)
        => principal.HasClaim("Organization", organization);

    public static bool HasInspector(this ClaimsPrincipal principal)
        => principal.HasClaim(x => x.Type == "Inspector");

    public static bool HasInspector(this ClaimsPrincipal principal, string inspector)
        => principal.HasClaim("Inspector", inspector);

    public static bool HasProof(this ClaimsPrincipal principal)
        => principal.HasClaim(x => x.Type == "Proof");

    public static bool HasProof(this ClaimsPrincipal principal, string proof)
        => principal.HasClaim("Proof", proof);

    public static bool HasImpersonatorOrganization(this ClaimsPrincipal principal)
        => principal.HasClaim(x => x.Type == "ImpersonatorOrganization");

    public static bool HasImpersonatorOrganization(this ClaimsPrincipal principal, string organization)
        => principal.HasClaim("ImpersonatorOrganization", organization);

    public static bool HasImpersonatorInspector(this ClaimsPrincipal principal)
        => principal.HasClaim(x => x.Type == "ImpersonatorInspector");

    public static bool HasImpersonatorInspector(this ClaimsPrincipal principal, string inspector)
        => principal.HasClaim("ImpersonatorInspector", inspector);

    public static bool HasAuthorization(this ClaimsPrincipal principal, string authorization)
        => principal.HasClaim("Authorization", authorization);

    public static bool HasAnyAuthorization(this ClaimsPrincipal principal, IEnumerable<string> authorizations)
        => principal.FindAll("Authorization")
            .Where(x => authorizations.Contains(x.Value))
            .Any(x => !principal.FindAll("AuthorizationFilter").Any() ||
                      principal.HasClaim("AuthorizationFilter", x.Value));

    public static bool HasAnyAuthorization(this ClaimsPrincipal principal, params string[] authorizations)
        => principal.HasAnyAuthorization(authorizations.AsEnumerable());

    public static bool HasAuthorizationFilter(this ClaimsPrincipal principal)
        => principal.HasClaim(x => x.Type == "AuthorizationFilter");

    public static bool HasAuthorizationFilter(this ClaimsPrincipal principal, string authorizationFilter)
        => principal.HasClaim("AuthorizationFilter", authorizationFilter);

    public static Token ToToken(this ClaimsPrincipal user)
    {
        var token = new Token()
        {
            Identity = user.HasIdentity()
                ? user.GetIdentity()
                : null,

            Organization = user.HasOrganization()
                ? user.GetOrganization()
                : null,

            Inspector = user.HasInspector()
                ? user.GetInspector()
                : null,
            Proof = user.HasProof()
                ? user.GetProof()
                : null,

            ImpersonatorOrganization = user.HasImpersonatorOrganization()
                ? user.GetImpersonatorOrganization()
                : null,

            ImpersonatorInspector = user.HasImpersonatorInspector()
                ? user.GetImpersonatorInspector()
                : null,

            Authorizations = user.GetAuthorizations(),
        };

        return token;
    }
}