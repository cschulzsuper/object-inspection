using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IEventStorage
    {
        ValueTask AddAsync(EventBase @event, ClaimsPrincipal user);

        IAsyncEnumerable<EventStorageEntry> GetPendingEventsAsync();

        ValueTask SetEventAsInProgressAsync(Guid eventId);

        ValueTask SetEventCompletionAsync(Guid eventId);

        ValueTask SetEventFailureAsync(Guid eventId, Exception? exception);
    }
}
