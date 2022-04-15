using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IEventProcessingStorage
    {
        ValueTask AddAsync(string SubscriberName, EventBase @event, ClaimsPrincipal user);

        IAsyncEnumerable<(string SubscriberName, EventBase Event, ClaimsPrincipal User)> GetPendingEventProcessingsAsync();

        ValueTask<bool> SetEventProcessingAsInProgressAsync(string subscriberName, Guid eventId);

        ValueTask SetEventProcessingCompletionAsync(string subscriberName, Guid eventId);

        ValueTask SetEventProcessingFailureAsync(string subscriberName, Guid eventId, Exception? exception);
    }
}
