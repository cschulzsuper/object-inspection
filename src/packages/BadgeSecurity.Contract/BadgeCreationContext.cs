using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

public class BadgeCreationContext
{
    public required ClaimsPrincipal User { get; init; }

    public required string AuthorizationType { get; init; }

    public required object AuthorizationResource { get; init; }

    public required string Proof { get; init; }
}