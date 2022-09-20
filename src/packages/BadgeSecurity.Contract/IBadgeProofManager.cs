using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

public interface IBadgeProofManager
{
    public string Create(BadgeProofAuthorizationContext context);

    public void Purge(ClaimsPrincipal user);

    public bool Verify(BadgeProofAuthenticationContext context);
}