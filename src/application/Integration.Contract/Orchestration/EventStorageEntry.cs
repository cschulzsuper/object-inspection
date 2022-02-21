using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public record EventStorageEntry(EventBase Event, ClaimsPrincipal User);
}
