using System.Collections.Generic;
using System.Security.Claims;

namespace ChristianSchulz.ObjectInspection.BadgeSecurity;

public interface IBadgeClaimsFactory
{
    public ICollection<Claim> Create(BadgeCreationContext context);
}
