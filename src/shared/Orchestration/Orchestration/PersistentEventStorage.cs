using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Application.Orchestration;
using Super.Paula.Shared.Authorization;

namespace Super.Paula.Shared.Orchestration;

public class PersistentEventStorage : IEventStorage
{
    private readonly ILogger<PersistentEventStorage> _logger;
    private readonly IEventManager _eventManager;
    private readonly IEventTypeRegistry _eventRegistry;
    private readonly EventAwaiter _eventAwaiter;

    public PersistentEventStorage(
        ILogger<PersistentEventStorage> logger,
        IEventManager eventManager,
        IEventTypeRegistry eventRegistry,
        EventAwaiter eventAwaiter)
    {
        _logger = logger;
        _eventManager = eventManager;
        _eventRegistry = eventRegistry;
        _eventAwaiter = eventAwaiter;
    }

    public async ValueTask AddAsync(EventBase @event, ClaimsPrincipal user)
    {
        var eventType = @event.GetType();
        var eventName = TypeNameConverter.ToKebabCase(eventType);

        var entity = new Event
        {
            Name = eventName,
            CreationTime = @event.CreationTime,
            CreationDate = @event.CreationDate,
            EventId = @event.Id.ToString(),
            Data = Base64Encoder.ObjectToBase64(@event),
            User = Base64Encoder.ObjectToBase64(user.ToToken()),
            OperationId = Activity.Current?.RootId ?? Guid.NewGuid().ToString(),
        };

        await _eventManager.InsertAsync(entity);

        _logger.LogInformation("An event has been added ({event}, {user})", @event, user);

        _eventAwaiter.Signal();
    }

    public async IAsyncEnumerable<(EventBase, ClaimsPrincipal)> GetPendingEventsAsync()
    {
        await _eventAwaiter.WaitAsync();

        // As soon as the ef core cosmos db provider can create composite indices this can be refactored.
        // The refactoring will put the order by into a call of GetAsyncEnumerable().
        //
        // https://github.com/dotnet/efcore/issues/17303 

        var events = _eventManager
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

            yield return (data, user);
        }
    }

    public async ValueTask<bool> SetEventAsInProgressAsync(Guid eventId)
    {
        var @event = _eventManager.GetQueryable()
            .SingleOrDefault(x => x.EventId == eventId.ToString());

        if (@event is not null and not { State: "in-progress" })
        {
            try
            {
                @event.State = "in-progress";
                await _eventManager.UpdateAsync(@event);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "An error occured while marking event as in progress ({eventId})", eventId);
                return false;
            }

            _logger.LogInformation("A event has been marked as in progress ({eventId})", eventId);
            return true;
        }
        else
        {
            _logger.LogWarning("Could not mark event as in progress ({eventId})", eventId);
            return false;
        }
    }

    public async ValueTask SetEventCompletionAsync(Guid eventId)
    {
        var @event = _eventManager.GetQueryable()
            .SingleOrDefault(x => x.EventId == eventId.ToString());

        if (@event is { State: "in-progress" })
        {
            await _eventManager.DeleteAsync(@event);

            _logger.LogInformation("A event has been marked as completed ({eventId})", eventId);
        }
        else
        {
            _logger.LogWarning("Could not mark event as completed ({eventId})", eventId);
        }
    }

    public async ValueTask SetEventFailureAsync(Guid eventId, Exception? exception)
    {
        var @event = _eventManager.GetQueryable()
            .SingleOrDefault(x => x.EventId == eventId.ToString());

        if (@event is not null and not { State: "in-progress" })
        {
            @event.State = "failed";
            await _eventManager.UpdateAsync(@event);

            _logger.LogWarning(exception, "An event has been marked as failed ({eventId})", eventId);

        }
        else
        {
            _logger.LogWarning(exception, "Could not mark event as failed ({eventId})", eventId);
        }
    }
}