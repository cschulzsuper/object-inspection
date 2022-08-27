using Super.Paula.Data;
using Super.Paula.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration;

public class EventProcessingManager : IEventProcessingManager
{
    private readonly IRepository<EventProcessing> _eventProcessingRepository;

    public EventProcessingManager(IRepository<EventProcessing> eventProcessingRepository)
    {
        _eventProcessingRepository = eventProcessingRepository;
    }

    public async ValueTask<EventProcessing> GetAsync(string id)
    {
        EnsureGetable(id);

        var entity = await _eventProcessingRepository.GetByIdsOrDefaultAsync(id);
        if (entity == null)
        {
            throw new ManagementException($"Event processing '{id}' was not found.");
        }

        return entity;
    }

    public async ValueTask InsertAsync(EventProcessing eventProcessing)
    {
        EnsureInsertable(eventProcessing);

        try
        {
            await _eventProcessingRepository.InsertAsync(eventProcessing);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not insert event processing '{eventProcessing.Name}'.", exception);
        }
    }

    public async ValueTask UpdateAsync(EventProcessing eventProcessing)
    {
        EnsureUpdateable(eventProcessing);

        try
        {
            await _eventProcessingRepository.UpdateAsync(eventProcessing);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not update event processing '{eventProcessing.EventId}'.", exception);
        }
    }

    public async ValueTask DeleteAsync(EventProcessing eventProcessing)
    {
        EnsureDeletable(eventProcessing);

        try
        {
            await _eventProcessingRepository.DeleteAsync(eventProcessing);
        }
        catch (Exception exception)
        {
            throw new ManagementException($"Could not delete event processing '{eventProcessing.EventId}'.", exception);
        }
    }

    public IQueryable<EventProcessing> GetQueryable()
        => _eventProcessingRepository.GetQueryable();

    public IAsyncEnumerable<EventProcessing> GetAsyncEnumerable()
        => _eventProcessingRepository.GetAsyncEnumerable();

    public IAsyncEnumerable<TResult> GetAsyncEnumerable<TResult>(Func<IQueryable<EventProcessing>, IQueryable<TResult>> query)
        => _eventProcessingRepository.GetAsyncEnumerable(query);

    private static void EnsureGetable(string id)
        => Validator.Ensure($"id '{id}' of event processing",
            EventProcessingValidator.IdIsNotEmpty(id),
            EventProcessingValidator.IdIsGuid(id));

    private static void EnsureInsertable(EventProcessing eventProcessing)
        => Validator.Ensure($"event processing with unique name '{eventProcessing.Name}'",
            EventProcessingValidator.IdIsNotEmpty(eventProcessing.EventId),
            EventProcessingValidator.IdIsGuid(eventProcessing.EventId),
            EventProcessingValidator.NameIsNotEmpty(eventProcessing.Name),
            EventProcessingValidator.NameHasKebabCase(eventProcessing.Name),
            EventProcessingValidator.NameIsNotTooLong(eventProcessing.Name),
            EventProcessingValidator.NameHasValidValue(eventProcessing.Name),
            EventProcessingValidator.SubscriberIsNotEmpty(eventProcessing.Subscriber),
            EventProcessingValidator.SubscriberHasKebabCase(eventProcessing.Subscriber),
            EventProcessingValidator.SubscriberIsNotTooLong(eventProcessing.Subscriber),
            EventProcessingValidator.SubscriberHasValidValue(eventProcessing.Subscriber),
            EventProcessingValidator.StateIsNotNull(eventProcessing.State),
            EventProcessingValidator.StateHasValidValue(eventProcessing.State),
            EventProcessingValidator.OperationIdIsNotEmpty(eventProcessing.OperationId),
            EventProcessingValidator.DataIsNotNull(eventProcessing.Data),
            EventProcessingValidator.DataIsNotTooLong(eventProcessing.Data),
            EventProcessingValidator.UserIsNotNull(eventProcessing.User),
            EventProcessingValidator.UserIsNotTooLong(eventProcessing.User),
            EventProcessingValidator.CreationDateIsPositive(eventProcessing.CreationDate),
            EventProcessingValidator.CreationTimeIsInDayTimeRange(eventProcessing.CreationTime));

    private static void EnsureUpdateable(EventProcessing eventProcessing)
        => Validator.Ensure($"event processing with id '{eventProcessing.EventId}'",
            EventProcessingValidator.IdIsNotEmpty(eventProcessing.EventId),
            EventProcessingValidator.IdIsGuid(eventProcessing.EventId),
            EventProcessingValidator.NameIsNotEmpty(eventProcessing.Name),
            EventProcessingValidator.NameHasKebabCase(eventProcessing.Name),
            EventProcessingValidator.NameIsNotTooLong(eventProcessing.Name),
            EventProcessingValidator.NameHasValidValue(eventProcessing.Name),
            EventProcessingValidator.SubscriberIsNotEmpty(eventProcessing.Subscriber),
            EventProcessingValidator.SubscriberHasKebabCase(eventProcessing.Subscriber),
            EventProcessingValidator.SubscriberIsNotTooLong(eventProcessing.Subscriber),
            EventProcessingValidator.SubscriberHasValidValue(eventProcessing.Subscriber),
            EventProcessingValidator.StateIsNotNull(eventProcessing.State),
            EventProcessingValidator.StateHasValidValue(eventProcessing.State),
            EventProcessingValidator.OperationIdIsNotEmpty(eventProcessing.OperationId),
            EventProcessingValidator.DataIsNotNull(eventProcessing.Data),
            EventProcessingValidator.DataIsNotTooLong(eventProcessing.Data),
            EventProcessingValidator.UserIsNotNull(eventProcessing.User),
            EventProcessingValidator.UserIsNotTooLong(eventProcessing.User),
            EventProcessingValidator.CreationDateIsPositive(eventProcessing.CreationDate),
            EventProcessingValidator.CreationTimeIsInDayTimeRange(eventProcessing.CreationTime));

    private static void EnsureDeletable(EventProcessing eventProcessing)
        => Validator.Ensure($"event processing with id '{eventProcessing.EventId}'",
            EventProcessingValidator.IdIsNotEmpty(eventProcessing.EventId),
            EventProcessingValidator.IdIsGuid(eventProcessing.EventId));
}