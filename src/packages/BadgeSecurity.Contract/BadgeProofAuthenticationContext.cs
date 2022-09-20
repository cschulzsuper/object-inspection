using System.Collections.Generic;
using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

public class BadgeProofAuthenticationContext
{
    public required ICollection<Claim> Claims { get; set; }
}