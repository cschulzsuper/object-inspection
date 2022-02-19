using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class InspectorEventHandler : IInspectorEventHandler
    {
        public async Task HandleAsync(EventHandlerContext context, OrganizationEvent @event)
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

        public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectorEvent @event)
        {
            var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();
            var inspectorAnnouncer = context.Services.GetRequiredService<IInspectorAnnouncer>();

            if (@event.OldInspector != null &&
                @event.OldInspector == @event.NewInspector)
            {
                var oldInspector = inspectorManager
                    .GetQueryable()
                    .SingleOrDefault(x => x.UniqueName == @event.OldInspector);

                if (oldInspector != null)
                {
                    var oldBusinessObjects = oldInspector.BusinessObjects
                        .Where(x => x.UniqueName == @event.UnqiueName);

                    foreach (var oldBusinessObject in oldBusinessObjects.Skip(1))
                    {
                        oldInspector.BusinessObjects.Remove(oldBusinessObject);
                    }

                    var changedBusinessObject = oldBusinessObjects.First();
                    changedBusinessObject.DisplayName = @event.DisplayName;

                    await inspectorManager.UpdateAsync(oldInspector);
                    await inspectorAnnouncer.AnnounceBusinessObjectUpdateAsync(@event.OldInspector, changedBusinessObject.ToResponse());
                }
            }

            var useBusinessObject = (InspectorBusinessObject?)null;

            if (@event.OldInspector != null &&
                @event.OldInspector != @event.NewInspector)
            {
                var oldInspector = inspectorManager
                    .GetQueryable()
                    .SingleOrDefault(x => x.UniqueName == @event.OldInspector);

                if (oldInspector != null)
                {
                    var oldBusinessObjects = oldInspector.BusinessObjects
                        .Where(x => x.UniqueName == @event.UnqiueName);

                    useBusinessObject = oldBusinessObjects.FirstOrDefault();

                    foreach (var oldBusinessObject in oldBusinessObjects)
                    {
                        oldInspector.BusinessObjects.Remove(oldBusinessObject);
                    }

                    await inspectorManager.UpdateAsync(oldInspector);
                    await inspectorAnnouncer.AnnounceBusinessObjectDeletionAsync(oldInspector.UniqueName, @event.UnqiueName);
                }
            }

            if (@event.OldInspector != @event.NewInspector &&
                @event.NewInspector != null)
            {
                var newInspector = inspectorManager
                    .GetQueryable()
                    .SingleOrDefault(x => x.UniqueName == @event.NewInspector);

                if (newInspector != null)
                {
                    var newBusinessObject = useBusinessObject ?? new InspectorBusinessObject();

                    newBusinessObject.DisplayName = @event.DisplayName;
                    newBusinessObject.UniqueName = @event.UnqiueName;

                    newInspector.BusinessObjects.Add(newBusinessObject);

                    await inspectorManager.UpdateAsync(newInspector);
                    await inspectorAnnouncer.AnnounceBusinessObjectCreationAsync(newInspector.UniqueName, newBusinessObject.ToResponse());
                }
            }
        }

        public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectionAuditScheduleEvent @event)
        {
            var inspectorManager = context.Services.GetRequiredService<IInspectorManager>();
            var inspectorAnnouncer = context.Services.GetRequiredService<IInspectorAnnouncer>();

            var inspector = inspectorManager
                .GetQueryable()
                .SingleOrDefault(x => x.UniqueName == @event.Inspector);

            if (inspector == null)
            {
                return;
            }

            var inspectorBusinessObject = inspector.BusinessObjects
                .Single(x => x.UniqueName == @event.UniqueName);

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
                var plannedAuditTimestamp = (@event.PlannedAuditDate, @event.PlannedAuditTime).ToDateTime();

                var oldDelayed = inspectorBusinessObject.AuditScheduleDelayed;
                var oldPending = inspectorBusinessObject.AuditSchedulePending;

                inspectorBusinessObject.AuditSchedulePlannedAuditDate = @event.PlannedAuditDate;
                inspectorBusinessObject.AuditSchedulePlannedAuditTime = @event.PlannedAuditTime;

                var now = DateTime.UtcNow;
                var threshold = @event.Threshold;

                inspectorBusinessObject.AuditScheduleDelayed = now > plannedAuditTimestamp.AddMilliseconds(threshold);
                inspectorBusinessObject.AuditSchedulePending = now > plannedAuditTimestamp.AddMilliseconds(-threshold);

                await inspectorManager.UpdateAsync(inspector);
                await inspectorAnnouncer.AnnounceBusinessObjectUpdateAsync(@event.Inspector, inspectorBusinessObject.ToResponse());

                await PublishInspectorBusinessObjectAsync(context, inspector, inspectorBusinessObject, oldDelayed, oldPending);
            }
        }

        private static async ValueTask PublishInspectorBusinessObjectAsync(
            EventHandlerContext context,
            Inspector inspector,
            InspectorBusinessObject inspectorBusinessObject,
            bool oldDelayed,
            bool oldPending)

        {
            var eventBus = context.Services.GetRequiredService<IEventBus>();

            var @event = new InspectorBusinessObjectEvent(
                inspector.UniqueName,
                inspectorBusinessObject.UniqueName,
                inspectorBusinessObject.DisplayName,
                inspectorBusinessObject.AuditScheduleDelayed,
                inspectorBusinessObject.AuditSchedulePending,
                oldDelayed,
                oldPending);

            await eventBus.PublishAsync(@event, context.User);
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
}