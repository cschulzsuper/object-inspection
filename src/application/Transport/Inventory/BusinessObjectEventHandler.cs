using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectEventHandler : IBusinessObjectEventHandler
    {
        public async Task HandleAsync(EventHandlerContext context, InspectionEvent @event)
        {
            var businessObjectManager = context.Services.GetRequiredService<IBusinessObjectManager>();

            var businessObjects = businessObjectManager.GetQueryableWhereInspection(@event.UniqueName)
                .AsEnumerable();

            foreach (var businessObject in businessObjects)
            {
                foreach (var businessObjectInspection in businessObject.Inspections)
                {
                    if (businessObjectInspection.UniqueName == @event.UniqueName)
                    {
                        businessObjectInspection.DisplayName = @event.DisplayName;
                        businessObjectInspection.Text = @event.Text;
                        businessObjectInspection.Activated = @event.Activated;
                    }
                }

                await businessObjectManager.UpdateAsync(businessObject);
            }
        }

        public async Task HandleAsync(EventHandlerContext context, InspectionDeletionEvent @event)
        {
            var businessObjectManager = context.Services.GetRequiredService<IBusinessObjectManager>();
            var businessObjectInspectionAuditScheduleFilter = context.Services.GetRequiredService<IBusinessObjectInspectionAuditScheduleFilter>();

            var businessObjects = businessObjectManager.GetQueryableWhereInspection(@event.UniqueName)
                .AsEnumerable();

            foreach (var businessObject in businessObjects)
            {
                var businessObjectInspections = businessObject.Inspections
                    .Where(x => x.UniqueName == @event.UniqueName)
                    .ToList();

                foreach (var businessObjectInspection in businessObjectInspections)
                {
                    businessObject.Inspections.Remove(businessObjectInspection);
                }

                await businessObjectManager.UpdateAsync(businessObject);

                await PublishBusinessObjectInspectionAuditScheduleAsync(context, businessObject);
            }
        }

        private async ValueTask PublishBusinessObjectInspectionAuditScheduleAsync(EventHandlerContext context, BusinessObject businessObject)
        {
            var eventBus = context.Services.GetRequiredService<IEventBus>();

            var inspection = businessObject.Inspections
                .Where(x => x.AuditSchedule.Appointments.Any())
                .OrderBy(x => x.AuditSchedule.Appointments
                    .Min(y => (y.PlannedAuditDate, y.PlannedAuditTime).ToDateTime()))
                .FirstOrDefault();

            if (inspection == null)
            {
                return;
            }

            var @event = new BusinessObjectInspectionAuditScheduleEvent(
                businessObject.UniqueName,
                businessObject.Inspector,
                inspection.AuditSchedule.Appointments.First().PlannedAuditDate,
                inspection.AuditSchedule.Appointments.First().PlannedAuditTime,
                inspection.AuditSchedule.Threshold);

            await eventBus.PublishAsync(@event, context.User);
        }
    }
}