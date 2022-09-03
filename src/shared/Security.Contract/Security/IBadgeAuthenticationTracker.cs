using System.Security.Claims;

namespace Super.Paula.Shared.Security;

public interface IBadgeAuthenticationTracker
{
    ClaimsPrincipal? Verify(string? encodedBadge);

    string Trace(ClaimsPrincipal user, string badgeType, object badgeResource);
    
    void Forget(ClaimsPrincipal user);
}