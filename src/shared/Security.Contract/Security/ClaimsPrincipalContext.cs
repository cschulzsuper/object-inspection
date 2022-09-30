using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.Shared.Security;

public class ClaimsPrincipalContext
{
    public ClaimsPrincipalContext()
    {
        User = new ClaimsPrincipal();
    }

    public ClaimsPrincipal User { get; internal set; }
}