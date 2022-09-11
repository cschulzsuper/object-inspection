using System.Security.Claims;

namespace Super.Paula.BadgeSecurity
{
    public interface IBadgeHandler
    {
        ClaimsPrincipal? Authenticate(string badge);

        string Authorize(ClaimsPrincipal user, string type, object resource);
    }
}
