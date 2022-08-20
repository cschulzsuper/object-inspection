using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Shared.Orchestration;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public class BusinessObjectInspectionAuditRecordEventHandler : IBusinessObjectInspectionAuditRecordEventHandler
{
    public async Task HandleAsync(EventHandlerContext context, BusinessObjectEvent @event)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.BusinessObject == @event.UniqueName)
            .Where(entity => entity.BusinessObjectDisplayName != @event.DisplayName)
            .AsEnumerable();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            businessObjectInspectionAuditRecord.BusinessObjectDisplayName = @event.DisplayName;

            await businessObjectInspectionAuditRecordManager.UpdateAsync(businessObjectInspectionAuditRecord);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, InspectionEvent @event)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.Inspection == @event.UniqueName)
            .Where(entity => entity.InspectionDisplayName != @event.DisplayName)
            .AsEnumerable();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            businessObjectInspectionAuditRecord.InspectionDisplayName = @event.DisplayName;

            await businessObjectInspectionAuditRecordManager.UpdateAsync(businessObjectInspectionAuditRecord);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, InspectionDeletionEvent @event)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.Inspection == @event.UniqueName)
            .AsEnumerable();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            await businessObjectInspectionAuditRecordManager.DeleteAsync(businessObjectInspectionAuditRecord);
        }
    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectDeletionEvent @event)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecords = businessObjectInspectionAuditRecordManager
            .GetQueryable()
            .Where(entity => entity.BusinessObject == @event.UniqueName)
            .AsEnumerable();

        foreach (var businessObjectInspectionAuditRecord in businessObjectInspectionAuditRecords)
        {
            await businessObjectInspectionAuditRecordManager.DeleteAsync(businessObjectInspectionAuditRecord);
        }
    }
}