using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class InMemoryEventStorage : IEventStorage
    {
        private readonly ConcurrentDictionary<Guid, EventStorageEntry> _events;
        private readonly ILogger<InMemoryEventStorage> _logger;

        public InMemoryEventStorage(ILogger<InMemoryEventStorage> logger)
        {
            _events = new ConcurrentDictionary<Guid, EventStorageEntry>();
            _logger  =logger;
        }

        public ValueTask AddAsync(EventBase @event, ClaimsPrincipal user)
        {
            var entry = new EventStorageEntry(@event, user);

            _events.AddOrUpdate(@event.Id, entry, (_,_) => entry);

            _logger.LogInformation("An event has been added ({event}, {user})", @event, user);
            return ValueTask.CompletedTask;
        }

        public async IAsyncEnumerable<EventStorageEntry> GetPendingEventsAsync()
        {
            await Task.CompletedTask;

            foreach(var @event in _events.Values)
            {
                yield return @event;
            }
        }

        public ValueTask SetEventAsInProgressAsync(Guid eventId)
        {
            var found = _events.TryGetValue(eventId, out var entry);

            if (found && entry != null)
            {
                _logger.LogInformation("An event has been marked as in progress ({event}, {user})", entry.Event, entry.User);
            }
            else
            {
                _logger.LogWarning("An unkown event has been marked as in progress ({eventId})", eventId);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask SetEventCompletionAsync(Guid eventId)
        {
            var removed = _events.TryRemove(eventId, out var entry);

            if (removed && entry != null)
            {
                _logger.LogInformation("An event has been marked as completed ({event}, {user})", entry.Event, entry.User);
            }
            else
            {
                _logger.LogWarning("An unkown event has been marked as completed ({eventId})", eventId);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask SetEventFailureAsync(Guid eventId, Exception? exception)
        {
            var removed = _events.TryRemove(eventId, out var entry);

            if (removed && entry != null)
            {
                _logger.LogWarning(exception, "An event has been marked as failed ({event}, {user})", entry.Event, entry.User);
               
            }
            else
            {
                _logger.LogWarning(exception, "An unkown event has been marked as failed ({eventId})", eventId);
            }

            return ValueTask.CompletedTask;
        }
    }
}
