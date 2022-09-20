using System.Collections.Generic;
using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

public interface IBadgeClaimsFilter
{
    void Apply(ICollection<Claim> claims);
}
