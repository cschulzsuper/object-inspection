using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Administration.Events;
using ChristianSchulz.ObjectInspection.Application.Administration.Responses;
using ChristianSchulz.ObjectInspection.Application.Auditing.Events;
using ChristianSchulz.ObjectInspection.Application.Inventory.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System;
using System.Linq;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Shared;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public class InspectorEventHandler : IInspectorEventHandler
{
    public async Task HandleAsync(EventHandlerContext context, OrganizationUpdateEvent @event)
    {
        var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();
        var identityInspectorManager = context.Services.GetRequiredService<IIdentityInspectorManager>();

        var inspectors = inspectorManager.GetQueryable()
            .Where(x => x.Organization == @event.UniqueName)
            .ToList();

        foreach (var inspector in inspectors)
        {
            inspector.OrganizationActivated = @event.Activated;
            inspector.OrganizationDisplayName =  @event.DisplayName;

            await inspectorManager.UpdateAsync(inspector);

            var identity = await identityInspectorManager.GetAsync(
                inspector.Identity,
                inspector.Organization,
                inspector.UniqueName);

            identity.Activated = inspector.Activated && inspector.OrganizationActivated;

            await identityInspectorManager.UpdateAsync(identity);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectorCreationEvent @event)
    {
        var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();
        var inspectorBroadcaster = context.Services.GetRequiredService<IInspectorBroadcaster>();

        var newInspector = inspectorManager
            .GetQueryable()
            .SingleOrDefault(x => x.UniqueName == @event.Inspector);

        if (newInspector != null)
        {
            var newBusinessObject = new InspectorBusinessObject
            {
                UniqueName = @event.UniqueName,
                DisplayName = @event.DisplayName
            };

            newInspector.BusinessObjects.Add(newBusinessObject);

            await inspectorManager.UpdateAsync(newInspector);
            await inspectorBroadcaster.SendInspectorBusinessObjectCreationAsync(newInspector.UniqueName, newBusinessObject.ToResponse());
        }
    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectorDeletionEvent @event)
    {
        var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();
        var inspectorBroadcaster = context.Services.GetRequiredService<IInspectorBroadcaster>();

        var oldInspector = inspectorManager
            .GetQueryable()
            .SingleOrDefault(x => x.UniqueName == @event.Inspector);

        if (oldInspector != null)
        {
            var oldBusinessObjects = oldInspector.BusinessObjects
                .Where(x => x.UniqueName == @event.UniqueName);

            foreach (var oldBusinessObject in oldBusinessObjects)
            {
                oldInspector.BusinessObjects.Remove(oldBusinessObject);
            }

            await inspectorManager.UpdateAsync(oldInspector);
            await inspectorBroadcaster.SendInspectorBusinessObjectDeletionAsync(oldInspector.UniqueName, @event.UniqueName);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectionAuditScheduleEvent @event)
    {
        var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();
        var inspectorBroadcaster = context.Services.GetRequiredService<IInspectorBroadcaster>();

        var inspectors = inspectorManager
            .GetQueryableWhereBusinessObject(@event.BusinessObject)
            .ToList();

        foreach (var inspector in inspectors)
        {
            var inspectorBusinessObject = inspector.BusinessObjects
                .Single(x => x.UniqueName == @event.BusinessObject);

            if (@event.PlannedAuditDate == default)
            {
                inspectorBusinessObject.AuditSchedulePlannedAuditDate = default;
                inspectorBusinessObject.AuditSchedulePlannedAuditTime = default;
                inspectorBusinessObject.AuditScheduleDelayed = false;
                inspectorBusinessObject.AuditSchedulePending = false;

                await inspectorManager.UpdateAsync(inspector);
            }
            else
            {
                var plannedAuditTimestamp =
                    new DateTimeNumbers(
                        @event.PlannedAuditDate,
                        @event.PlannedAuditTime)
                    .ToGlobalDateTime();

                var oldDelayed = inspectorBusinessObject.AuditScheduleDelayed;
                var oldPending = inspectorBusinessObject.AuditSchedulePending;

                inspectorBusinessObject.AuditSchedulePlannedAuditDate = @event.PlannedAuditDate;
                inspectorBusinessObject.AuditSchedulePlannedAuditTime = @event.PlannedAuditTime;

                var now = DateTime.UtcNow;
                var threshold = @event.Threshold;

                inspectorBusinessObject.AuditScheduleDelayed = now > plannedAuditTimestamp.AddMilliseconds(threshold);
                inspectorBusinessObject.AuditSchedulePending = now > plannedAuditTimestamp.AddMilliseconds(-threshold);

                await inspectorManager.UpdateAsync(inspector);
                await inspectorBroadcaster.SendInspectorBusinessObjectUpdateAsync(inspector.UniqueName, inspectorBusinessObject.ToResponse());

                var inspectorEventService = context.Services.GetRequiredService<IInspectorEventService>();

                if (inspectorBusinessObject.AuditScheduleDelayed && 
                    inspectorBusinessObject.AuditScheduleDelayed != oldDelayed)
                {
                    await inspectorEventService.CreateInspectorBusinessObjectOverdueDetectionEventAsync(inspector, inspectorBusinessObject);
                }
                else
                {

                    if (inspectorBusinessObject.AuditSchedulePending &&
                        inspectorBusinessObject.AuditSchedulePending != oldPending)
                    {
                        await inspectorEventService.CreateInspectorBusinessObjectImmediacyDetectionEventAsync(inspector, inspectorBusinessObject);
                    }
                }
            }
        }
    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectUpdateEvent @event)
    {
        var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();

        var inspectors = inspectorManager
            .GetQueryableWhereBusinessObject(@event.UniqueName)
            .ToList();

        foreach (var inspector in inspectors)
        {
            var businessObjects = inspector.BusinessObjects
                .Where(x => x.UniqueName == @event.UniqueName)
                .ToList();

            foreach (var businessObject in businessObjects)
            {
                businessObject.DisplayName = @event.DisplayName;
            }

            await inspectorManager.UpdateAsync(inspector);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectDeletionEvent @event)
    {
        var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();

        var inspectors = inspectorManager
            .GetQueryableWhereBusinessObject(@event.UniqueName)
            .ToList();

        if (!inspectors.Any())
        {
            return;
        }

        foreach (var inspector in inspectors)
        {
            var businessObjects = inspector.BusinessObjects
                .Where(x => x.UniqueName == @event.UniqueName)
                .ToList();

            foreach (var businessObject in businessObjects)
            {
                inspector.BusinessObjects.Remove(businessObject);
            }

            await inspectorManager.UpdateAsync(inspector);
        }
    }
}