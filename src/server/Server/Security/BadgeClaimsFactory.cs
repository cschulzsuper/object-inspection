using System;
using System.Collections.Generic;
using System.Security.Claims;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Authentication;
using Super.Paula.BadgeSecurity;
using Super.Paula.Shared.Security;

namespace Super.Paula.Server.Security;

public class BadgeClaimsFactory : IBadgeClaimsFactory
{
    public ICollection<Claim> Create(BadgeCreationContext context)
    {
        var claims = context.AuthorizationType switch
        {
            "identity" => CreateIdentityBadge(context),
            "inspector" => CreateInspectorBadge(context),
            "impersonation" => CreateImpersonatorBadge(context),
            _ => Array.Empty<Claim>()
        };

        return claims;
    }

    private static ICollection<Claim> CreateIdentityBadge(BadgeCreationContext context)
    {
        var identity = (Identity)context.AuthorizationResource;

        var claims = new List<Claim>
        {
            new Claim("Identity",identity.UniqueName),
            new Claim("Proof",context.Proof)
        };

        return claims;
    }

    public static ICollection<Claim> CreateInspectorBadge(BadgeCreationContext context)
    {
        var identityInspector = (IdentityInspector)context.AuthorizationResource;

        var claims = new List<Claim>
        {
            new Claim("Identity",identityInspector.UniqueName),
            new Claim("Proof",context.Proof),
            new Claim("Inspector",identityInspector.Inspector),
            new Claim("Organization",identityInspector.Organization)
        };

        return claims;
    }

    private static ICollection<Claim> CreateImpersonatorBadge(BadgeCreationContext context)
    {
        var inspector = (Inspector)context.AuthorizationResource;

        var claims = new List<Claim>
        {
            new Claim("Identity",context.User.Claims.GetIdentity()),
            new Claim("Proof",context.Proof),
            new Claim("ImpersonatorInspector",context.User.Claims.GetInspector()),
            new Claim("ImpersonatorOrganization",context.User.Claims.GetOrganization()),
            new Claim("Inspector",inspector.UniqueName),
            new Claim("Organization",inspector.Organization)
        };

        return claims;
    }
}