﻿using ChristianSchulz.ObjectInspection.Application.Administration.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class InspectorEventService : IInspectorEventService
{
    private readonly IEventStorage _eventStorage;
    private readonly ClaimsPrincipal _user;

    public InspectorEventService(
        IEventStorage eventStorage,
        ClaimsPrincipal user)
    {
        _eventStorage = eventStorage;
        _user = user;
    }

    public async ValueTask CreateInspectorBusinessObjectImmediacyDetectionEventAsync(
        Inspector inspector,
        InspectorBusinessObject inspectorBusinessObject)

    {
        var @event = new InspectorBusinessObjectImmediacyDetectionEvent(
            inspector.UniqueName,
            inspectorBusinessObject.UniqueName,
            inspectorBusinessObject.DisplayName);

        await _eventStorage.AddAsync(@event, _user);
    }

    public async ValueTask CreateInspectorBusinessObjectOverdueDetectionEventAsync(Inspector inspector, InspectorBusinessObject inspectorBusinessObject)
    {
        var @event = new InspectorBusinessObjectOverdueDetectionEvent(
            inspector.UniqueName,
            inspectorBusinessObject.UniqueName,
            inspectorBusinessObject.DisplayName);

        await _eventStorage.AddAsync(@event, _user);
    }
}