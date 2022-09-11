using System.Collections.Generic;
using System.Security.Claims;

namespace Super.Paula.BadgeUsage;

public interface IBadgeEncoding
{
    ICollection<Claim> Decode(string badge);
    string Encode(IEnumerable<Claim> badgeClaims);
}