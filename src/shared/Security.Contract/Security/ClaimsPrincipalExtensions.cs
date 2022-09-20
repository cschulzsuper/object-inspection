using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.Shared.Security;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAuthenticatedInspector(this ClaimsPrincipal principal)
        => principal.Identity?.IsAuthenticated == true &&
           principal.Claims.HasInspector();

    public static bool IsAuthenticatedIdentity(this ClaimsPrincipal principal)
        => principal.Identity?.IsAuthenticated == true &&
           principal.Claims.HasIdentity();

}