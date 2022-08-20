using System.Security.Claims;

namespace Super.Paula.Shared.Authorization;

public class ClaimsPrincipalContext
{
    public ClaimsPrincipalContext()
    {
        User = new ClaimsPrincipal();
    }

    public ClaimsPrincipal User { get; internal set; }
}