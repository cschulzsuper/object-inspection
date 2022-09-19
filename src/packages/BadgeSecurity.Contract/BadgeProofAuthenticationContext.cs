using System.Collections.Generic;
using System.Security.Claims;

namespace Super.Paula.BadgeSecurity;

public class BadgeProofAuthenticationContext
{
    public required ICollection<Claim> Claims { get; set; }
}