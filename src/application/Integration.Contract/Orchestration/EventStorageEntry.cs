using System.Security.Claims;

namespace Super.Paula.Application.Orchestration
{
    public record EventStorageEntry(EventBase Event, ClaimsPrincipal User);
}
