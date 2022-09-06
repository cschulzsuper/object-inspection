﻿using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Shared.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory;

public class BusinessObjectEventService : IBusinessObjectEventService
{
    private readonly ClaimsPrincipal _user;
    private readonly IEventStorage _eventStorage;

    public BusinessObjectEventService(
        ClaimsPrincipal user,
        IEventStorage eventStorage)
    {
        _user = user;
        _eventStorage = eventStorage;
    }

    public async ValueTask CreateBusinessObjectUpdateEventAsync(BusinessObject businessObject)
    {
        var @event = new BusinessObjectUpdateEvent(
            businessObject.UniqueName,
            businessObject.DisplayName);

        await _eventStorage.AddAsync(@event, _user);
    }

    public async ValueTask CreateBusinessObjectDeletionEventAsync(string businessObject)
    {
        var @event = new BusinessObjectDeletionEvent(businessObject);

        await _eventStorage.AddAsync(@event, _user);
    }
}