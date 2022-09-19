using System.Linq;
using System.Security.Claims;
using Super.Paula.BadgeUsage;

namespace Super.Paula.BadgeSecurity;

public class BadgeHandler : IBadgeHandler
{
    private readonly IBadgeProofManager _badgeProofManager;
    private readonly IBadgeEncoding _badgeEncoding;
    private readonly IBadgeClaimsFilter _badgeClaimsFilter;
    private readonly IBadgeClaimsFactory _badgeClaimsFactory;

    public BadgeHandler(
        IBadgeProofManager badgeProofManager,
        IBadgeEncoding badgeEncoding,
        IBadgeClaimsFilter badgeClaimsFilter,
        IBadgeClaimsFactory badgeClaimsFactory)
    {
        _badgeProofManager = badgeProofManager;
        _badgeEncoding = badgeEncoding;
        _badgeClaimsFilter = badgeClaimsFilter;
        _badgeClaimsFactory = badgeClaimsFactory;
    }

    public string Authorize(ClaimsPrincipal user, string authorizationType, object authorizationResource)
    {
        var context = new BadgeProofAuthorizationContext
        {
            AuthorizationResource = authorizationResource,
            AuthorizationType = authorizationType,
            User = user
        };

        var badgeProof = _badgeProofManager.Create(context);
        var badgeCreationContext = new BadgeCreationContext
        {
            AuthorizationResource = authorizationResource,
            AuthorizationType = authorizationType,
            User = user,
            Proof = badgeProof
        };

        var badgeClaims = _badgeClaimsFactory.Create(badgeCreationContext);

        _badgeClaimsFilter.Apply(badgeClaims);

        return _badgeEncoding.Encode(badgeClaims);
    }

    public ClaimsPrincipal? Authenticate(string badge)
    {
        if (string.IsNullOrWhiteSpace(badge))
        {
            return null;
        }

        var badgeClaims = _badgeEncoding.Decode(badge);
        if (!badgeClaims.Any())
        {
            return null;
        }

        var badgeAuthenticationContext = new BadgeProofAuthenticationContext
        {
            Claims = badgeClaims
        };

        var badgeValid = _badgeProofManager.Verify(badgeAuthenticationContext);
        if (!badgeValid)
        {
            return null;
        }

        _badgeClaimsFilter.Apply(badgeClaims);
        
        var claimsIdentity = new ClaimsIdentity(badgeClaims, "badge");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }
}