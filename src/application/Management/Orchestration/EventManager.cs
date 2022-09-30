using ChristianSchulz.ObjectInspection.Data;
using ChristianSchulz.ObjectInspection.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Orchestration;

public class EventManager : IEventManager
{
    private readonly IRepository<Event> _eventRepository;

    public EventManager(IRepository<Event> eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async ValueTask<Event> GetAsync(string id)
    {
        EnsureGetable(id);

        var entity = await _eventRepository.GetByIdsOrDefaultAsync(id);
        if (entity == null)
        {
            throw new ManagementException($"Event '{id}' was not found.");
        }

        return entity;
    }

    public async ValueTask InsertAsync(Event @event)
    {
        EnsureInsertable(@event);

        try
        {
            await _eventRepository.InsertAsync(@event);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not insert event '{@event.Name}'.", exception);
        }
    }

    public async ValueTask UpdateAsync(Event @event)
    {
        EnsureUpdateable(@event);

        try
        {
            await _eventRepository.UpdateAsync(@event);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not update event '{@event.EventId}'.", exception);
        }
    }

    public async ValueTask DeleteAsync(Event @event)
    {
        EnsureDeletable(@event);

        try
        {
            await _eventRepository.DeleteAsync(@event);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not delete event '{@event.EventId}'.", exception);
        }
    }

    public IQueryable<Event> GetQueryable()
        => _eventRepository.GetQueryable();

    public IAsyncEnumerable<Event> GetAsyncEnumerable()
        => _eventRepository.GetAsyncEnumerable();

    public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<Event>, IQueryable<TResult>> query)
        => _eventRepository.GetAsyncEnumerable(query);

    private static void EnsureGetable(string id)
        => Validator.Ensure($"id '{id}' of event",
            EventValidator.IdIsNotEmpty(id),
            EventValidator.IdIsGuid(id));

    private static void EnsureInsertable(Event @event)
        => Validator.Ensure($"event with unique name '{@event.Name}'",
            EventValidator.IdIsNotEmpty(@event.EventId),
            EventValidator.IdIsGuid(@event.EventId),
            EventValidator.NameIsNotEmpty(@event.Name),
            EventValidator.NameHasKebabCase(@event.Name),
            EventValidator.NameIsNotTooLong(@event.Name),
            EventValidator.NameHasValidValue(@event.Name),
            EventValidator.StateIsNotNull(@event.State),
            EventValidator.StateHasValidValue(@event.State),
            EventValidator.OperationIdIsNotEmpty(@event.OperationId),
            EventValidator.DataIsNotNull(@event.Data),
            EventValidator.DataIsNotTooLong(@event.Data),
            EventValidator.UserIsNotNull(@event.User),
            EventValidator.UserIsNotTooLong(@event.User),
            EventValidator.CreationDateIsPositive(@event.CreationDate),
            EventValidator.CreationTimeIsInDayTimeRange(@event.CreationTime));

    private static void EnsureUpdateable(Event @event)
        => Validator.Ensure($"event with id '{@event.EventId}'",
            EventValidator.IdIsNotEmpty(@event.EventId),
            EventValidator.IdIsGuid(@event.EventId),
            EventValidator.NameIsNotEmpty(@event.Name),
            EventValidator.NameHasKebabCase(@event.Name),
            EventValidator.NameIsNotTooLong(@event.Name),
            EventValidator.NameHasValidValue(@event.Name),
            EventValidator.StateIsNotNull(@event.State),
            EventValidator.StateHasValidValue(@event.State),
            EventValidator.OperationIdIsNotEmpty(@event.OperationId),
            EventValidator.DataIsNotNull(@event.Data),
            EventValidator.DataIsNotTooLong(@event.Data),
            EventValidator.UserIsNotNull(@event.User),
            EventValidator.UserIsNotTooLong(@event.User),
            EventValidator.CreationDateIsPositive(@event.CreationDate),
            EventValidator.CreationTimeIsInDayTimeRange(@event.CreationTime));

    private static void EnsureDeletable(Event @event)
        => Validator.Ensure($"event with id '{@event.EventId}'",
            EventValidator.IdIsNotEmpty(@event.EventId),
            EventValidator.IdIsGuid(@event.EventId));
}