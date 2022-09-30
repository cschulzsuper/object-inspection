using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.BadgeSecurity;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Server.Security;

public class BadgeClaimsFilter : IBadgeClaimsFilter
{
    private readonly IOrganizationManager _organizationManager;
    private readonly AppSettings _appSettings;

    public BadgeClaimsFilter(
        IOrganizationManager organizationManager,
        AppSettings appSettings)
    {
        _organizationManager = organizationManager;
        _appSettings = appSettings;
    }

    public void Apply(ICollection<Claim> claims)
    {
        RemoveOldAuthorizationClaims(claims);

        AddNewAuthorizationClaims(claims);
    
    }

    private void RemoveOldAuthorizationClaims(ICollection<Claim> claims)
    {
        var authorizationClaims = claims
            .Where(x => x.Type == "Authorization")
            .ToList();

        foreach (var authorizationClaim in authorizationClaims)
        {
            claims.Remove(authorizationClaim);
        }
    }

    private void AddNewAuthorizationClaims(ICollection<Claim> claims)
    {
        var authorizations = new HashSet<string>();

        if (claims.HasOrganization())
        {
            if (_appSettings.DemoIdentity == claims.GetIdentity())
            {
                authorizations.Add("Observer");
            }
            else if (claims.HasInspector())
            {
                authorizations.Add("Inspector");
            }

            var organization = _organizationManager.Get(claims.GetOrganization());

            if (organization.ChiefInspector == claims.GetIdentity())
            {
                authorizations.Add("Chief");
            }

            if (claims.HasImpersonatorOrganization() &&
                claims.HasImpersonatorInspector())
            {
                authorizations.Add("Impersonator");
            }
        }

        if (_appSettings.MaintainerIdentity == claims.GetIdentity() &&
            !authorizations.Contains("Observer") &&
            !authorizations.Contains("Impersonator"))
        {
            authorizations.Add("Maintainer");
        }

        foreach (var authorization in authorizations)
        {
            claims.Add(new Claim("Authorization", authorization));
        }
    }
}