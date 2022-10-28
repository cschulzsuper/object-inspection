using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Guidelines.Events;
using ChristianSchulz.ObjectInspection.Application.Inventory.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Linq;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspectionAuditRecordEventHandler : IBusinessObjectInspectionAuditRecordEventHandler
{
    public async Task HandleAsync(EventHandlerContext context, BusinessObjectUpdateEvent @event)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.BusinessObject == @event.UniqueName)
            .ToList();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            businessObjectInspectionAuditRecord.BusinessObjectDisplayName = @event.DisplayName;

            await businessObjectInspectionAuditRecordManager.UpdateAsync(businessObjectInspectionAuditRecord);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectDeletionEvent @event)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.BusinessObject == @event.UniqueName)
            .ToList();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            await businessObjectInspectionAuditRecordManager.DeleteAsync(businessObjectInspectionAuditRecord);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, InspectionUpdateEvent updateEvent)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.Inspection == updateEvent.UniqueName)
            .ToList();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            businessObjectInspectionAuditRecord.InspectionDisplayName = updateEvent.DisplayName;

            await businessObjectInspectionAuditRecordManager.UpdateAsync(businessObjectInspectionAuditRecord);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, InspectionDeletionEvent @event)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.Inspection == @event.UniqueName)
            .ToList();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            await businessObjectInspectionAuditRecordManager.DeleteAsync(businessObjectInspectionAuditRecord);
        }
    }
}