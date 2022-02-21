using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System;
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

                    businessObjectInspectionAuditScheduleFilter.Apply(
                        new BusinessObjectInspectionAuditScheduleFilterContext(
                            Inspection: businessObjectInspection,
                            Limit: DateTime.UtcNow.AddMonths(1).ToNumbers()));
                }

                await businessObjectManager.UpdateAsync(businessObject);

                var businessObjectEventService = context.Services.GetRequiredService<IBusinessObjectEventService>();
                await businessObjectEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(businessObject);
            }
        }
    }
}