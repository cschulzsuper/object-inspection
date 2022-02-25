using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Orchestration;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionEventHandler : IBusinessObjectInspectionEventHandler
    {
        public async Task HandleAsync(EventHandlerContext context, InspectionEvent @event)
        {
            var businessObjectInspectionManager = context.Services.GetRequiredService<IBusinessObjectInspectionManager>();

            var businessObjectInspections = businessObjectInspectionManager.GetQueryable()
                .Where(x => x.Inspection == @event.UniqueName)
                .AsEnumerable();

            foreach (var businessObjectInspection in businessObjectInspections)
            {
                businessObjectInspection.InspectionDisplayName = @event.DisplayName;
                businessObjectInspection.InspectionText = @event.Text;
                businessObjectInspection.Activated = @event.Activated;

                await businessObjectInspectionManager.UpdateAsync(businessObjectInspection);
            }
        }

        public async Task HandleAsync(EventHandlerContext context, InspectionDeletionEvent @event)
        {
            var businessObjectInspectionManager = context.Services.GetRequiredService<IBusinessObjectInspectionManager>();
            var businessObjectInspectionAuditScheduleFilter = context.Services.GetRequiredService<IBusinessObjectInspectionAuditScheduleFilter>();

            var businessObjects = businessObjectInspectionManager.GetQueryable()
                .Where(x => x.Inspection == @event.UniqueName)
                .AsEnumerable()
                .GroupBy(x => x.BusinessObject);

            foreach (var businessObject in businessObjects)
            {
                var businessObjectInspections = businessObject
                    .Where(x => x.Inspection == @event.UniqueName)
                    .ToList();

                foreach (var businessObjectInspection in businessObjectInspections)
                {
                    businessObjectInspections.Remove(businessObjectInspection);

                    await businessObjectInspectionManager.DeleteAsync(businessObjectInspection);
                }

                var businessObjectInspectionEventService = context.Services.GetRequiredService<IBusinessObjectInspectionEventService>();
                await businessObjectInspectionEventService.CreateBusinessObjectInspectionAuditScheduleEventAsync(businessObjectInspections);
            }
        }
    }
}