using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Guidelines.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspectionEventHandler : IBusinessObjectInspectionEventHandler
{
    public async Task HandleAsync(EventHandlerContext context, InspectionUpdateEvent updateEvent)
    {
        var businessObjectInspectionManager = context.Services.GetRequiredService<IBusinessObjectInspectionManager>();

        var businessObjectInspections = businessObjectInspectionManager.GetQueryable()
            .Where(x => x.Inspection == updateEvent.UniqueName)
            .ToList();

        foreach (var businessObjectInspection in businessObjectInspections)
        {
            businessObjectInspection.InspectionDisplayName = updateEvent.DisplayName;
            businessObjectInspection.InspectionText = updateEvent.Text;
            businessObjectInspection.Activated = updateEvent.Activated;

            await businessObjectInspectionManager.UpdateAsync(businessObjectInspection);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, InspectionDeletionEvent @event)
    {
        var businessObjectInspectionManager = context.Services.GetRequiredService<IBusinessObjectInspectionManager>();

        var businessObjects = businessObjectInspectionManager.GetQueryable()
            .Where(x => x.Inspection == @event.UniqueName)
            .ToList()
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