using System.Security.Claims;

namespace Super.Paula.BadgeSecurity;

public class BadgeCreationContext
{
    public required ClaimsPrincipal User { get; init; }

    public required string AuthorizationType { get; init; }

    public required object AuthorizationResource { get; init; }

    public required string Proof { get; init; }
}