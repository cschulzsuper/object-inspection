using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

public interface IBadgeHandler
{
    ClaimsPrincipal? Authenticate(string badge);

    string Authorize(ClaimsPrincipal user, string type, object resource);
}
