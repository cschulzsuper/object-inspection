using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Authorization;

namespace Super.Paula.Application.Orchestration
{
    public class PersistentEventProcessingStorage : IEventProcessingStorage
    {
        private readonly ILogger<PersistentEventProcessingStorage> _logger;
        private readonly IEventProcessingManager _eventProcessingManager;
        private readonly IEventTypeRegistry _eventRegistry;
        private readonly EventProcessingAwaiter _eventProcessingAwaiter;

        public PersistentEventProcessingStorage(
            ILogger<PersistentEventProcessingStorage> logger,
            IEventProcessingManager eventProcessingManager,
            IEventTypeRegistry eventRegistry,
            EventProcessingAwaiter eventProcessingAwaiter)
        {
            _logger = logger;
            _eventProcessingManager = eventProcessingManager;
            _eventRegistry = eventRegistry;
            _eventProcessingAwaiter = eventProcessingAwaiter;
        }

        public async ValueTask AddAsync(string subscriberName, EventBase @event, ClaimsPrincipal user)
        {
            var eventType = @event.GetType();
            var eventName = TypeNameConverter.ToKebabCase(eventType);

            var entity = new EventProcessing
            {
                Name = eventName,
                Subscriber = subscriberName,
                CreationTime = @event.CreationTime,
                CreationDate = @event.CreationDate,
                Id = @event.Id.ToString(),
                Data = Base64Encoder.ObjectToBase64(@event),
                User = Base64Encoder.ObjectToBase64(user.ToToken()),
                OperationId = Activity.Current?.RootId ?? Guid.NewGuid().ToString(),
            };

            await _eventProcessingManager.InsertAsync(entity);

            _logger.LogInformation("An event processing has been added ({subscriberName},{event},{user})", subscriberName, @event, user);

            _eventProcessingAwaiter.Signal();
        }

        public async IAsyncEnumerable<(string, EventBase, ClaimsPrincipal)> GetPendingEventProcessingsAsync()
        {
            await _eventProcessingAwaiter.WaitAsync();

            // As soon as the ef core cosmos db provider can create composite indices this can be refactored.
            // The refactoring will put the order by into a call of GetAsyncEnumerable().
            //
            // https://github.com/dotnet/efcore/issues/17303 

            var events = _eventProcessingManager
                .GetQueryable()
                .Where(x => x.State == string.Empty)
                .AsEnumerable()
                .OrderBy(x => x.CreationDate)
                .ThenBy(x => x.CreationTime);

            foreach (var @event in events)
            {
                await Task.CompletedTask;

                var eventType = _eventRegistry.GetEventType(@event.Name);

                if (eventType == null)
                {
                    _logger.LogWarning("Could not get event type for a event ({eventName}).", @event);
                    continue;
                }

                var data = (EventBase)Base64Encoder.Base64ToObject(@event.Data, eventType);

                var user = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        Base64Encoder.Base64ToObject<Token>(@event.User).ToClaims()));

                yield return (@event.Subscriber, data, user);
            }
        }

        public async ValueTask<bool> SetEventProcessingAsInProgressAsync(string subscriberName, Guid eventId)
        {
            var @event = _eventProcessingManager.GetQueryable()
                .SingleOrDefault(x => 
                    x.Subscriber == subscriberName &&
                    x.Id == eventId.ToString());

            if (@event is not null and not { State: "in-progress" })
            {
                try
                {
                    @event.State = "in-progress";
                    await _eventProcessingManager.UpdateAsync(@event);
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(exception, "An error occured while marking event processing as in progress ({subscriber},{eventId})", subscriberName, eventId);
                    return false;
                }

                _logger.LogInformation("A event processing has been marked as in progress ({subscriber},{eventId})", subscriberName, eventId);
                return true;
            }
            else
            {
                _logger.LogWarning("Could not mark event processing as in progress ({subscriber},{eventId})", subscriberName, eventId);
                return false;
            }
        }

        public async ValueTask SetEventProcessingCompletionAsync(string subscriberName, Guid eventId)
        {
            var @event = _eventProcessingManager.GetQueryable()
                .SingleOrDefault(x =>
                    x.Subscriber == subscriberName &&
                    x.Id == eventId.ToString());

            if (@event is { State: "in-progress" })
            {
                await _eventProcessingManager.DeleteAsync(@event);

                _logger.LogInformation("A event processing has been marked as completed ({subscriber},{eventId})", subscriberName, eventId);
            }
            else
            {
                _logger.LogWarning("Could not mark event processing as completed ({subscriber},{eventId})", subscriberName, eventId);
            }
        }

        public async ValueTask SetEventProcessingFailureAsync(string subscriberName, Guid eventId, Exception? exception)
        {
            var @event = _eventProcessingManager.GetQueryable()
                .SingleOrDefault(x =>
                    x.Subscriber == subscriberName &&
                    x.Id == eventId.ToString());

            if (@event is { State: "in-progress" })
            {
                @event.State = "failed";
                await _eventProcessingManager.UpdateAsync(@event);

                _logger.LogWarning(exception, "An event processing has been marked as failed ({subscriber},{eventId})", subscriberName, eventId);
            }
            else
            {
                _logger.LogWarning(exception, "Could not mark event processing as failed ({subscriber},{eventId})", subscriberName, eventId);
            }
        }
    }
}
