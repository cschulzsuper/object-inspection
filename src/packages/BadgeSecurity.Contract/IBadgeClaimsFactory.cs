using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.BadgeSecurity
{
    public interface IBadgeClaimsFactory
    {
        public ICollection<Claim> Create(BadgeCreationContext context);
    }
}
