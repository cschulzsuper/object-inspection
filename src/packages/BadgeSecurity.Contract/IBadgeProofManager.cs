using System.Security.Claims;

namespace Super.Paula.BadgeSecurity
{
    public interface IBadgeProofManager
    {
        public string Create(BadgeProofAuthorizationContext context);

        public void Purge(ClaimsPrincipal user);

        public bool Verify(BadgeProofAuthenticationContext context);
    }
}