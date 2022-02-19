using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionAuditEventHandler : IBusinessObjectInspectionAuditEventHandler
    {
        public async Task HandleAsync(EventHandlerContext context, BusinessObjectEvent @event)
        {
            var businessObjectInspectionAuditManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditManager>();

            var businessObjectInspectionAudits = businessObjectInspectionAuditManager
                .GetQueryable()
                .Where(entity => entity.BusinessObject == @event.UniqueName)
                .Where(entity => entity.BusinessObjectDisplayName != @event.DisplayName)
                .AsEnumerable();

            foreach (var businessObjectInspectionAudit in businessObjectInspectionAudits)
            {
                businessObjectInspectionAudit.BusinessObjectDisplayName = @event.DisplayName;

                await businessObjectInspectionAuditManager.UpdateAsync(businessObjectInspectionAudit);
            }
        }

        public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectionAuditEvent @event)
        {
            var businessObjectInspectionAuditManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditManager>();

            var businessObjectInspectionAudit = await businessObjectInspectionAuditManager.GetOrDefaultAsync(
                    @event.UniqueName,
                    @event.Inspection,
                    @event.AuditDate,
                    @event.AuditTime);

            if (businessObjectInspectionAudit != null)
            {
                businessObjectInspectionAudit.InspectionDisplayName = @event.InspectionDisplayName;
                businessObjectInspectionAudit.Annotation = @event.AuditAnnotation;
                businessObjectInspectionAudit.Result = @event.AuditResult;
                businessObjectInspectionAudit.BusinessObjectDisplayName = @event.DisplayName;
                businessObjectInspectionAudit.Inspector = @event.AuditInspector;

                await businessObjectInspectionAuditManager.UpdateAsync(businessObjectInspectionAudit);
            }
            else
            {
                businessObjectInspectionAudit = new BusinessObjectInspectionAudit
                {
                    Annotation = @event.AuditAnnotation,
                    AuditDate = @event.AuditDate,
                    AuditTime = @event.AuditTime,
                    BusinessObject = @event.UniqueName,
                    BusinessObjectDisplayName = @event.DisplayName,
                    Inspection = @event.Inspection,
                    InspectionDisplayName = @event.InspectionDisplayName,
                    Inspector = @event.AuditInspector,
                    Result = @event.AuditResult
                };

                await businessObjectInspectionAuditManager.InsertAsync(businessObjectInspectionAudit);
            }
        }

        public async Task HandleAsync(EventHandlerContext context, InspectionEvent @event)
        {
            var businessObjectInspectionAuditManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditManager>();

            var businessObjectInspectionAudits = businessObjectInspectionAuditManager
                .GetQueryable()
                .Where(entity => entity.Inspection == @event.UniqueName)
                .Where(entity => entity.InspectionDisplayName != @event.DisplayName)
                .AsEnumerable();

            foreach (var businessObjectInspectionAudit in businessObjectInspectionAudits)
            {
                businessObjectInspectionAudit.InspectionDisplayName = @event.DisplayName;

                await businessObjectInspectionAuditManager.UpdateAsync(businessObjectInspectionAudit);
            }
        }

        public async Task HandleAsync(EventHandlerContext context, InspectionDeletionEvent @event)
        {
            var businessObjectInspectionAuditManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditManager>();

            var businessObjectInspectionAudits = businessObjectInspectionAuditManager
                .GetQueryable()
                .Where(entity => entity.Inspection == @event.UniqueName)
                .AsEnumerable();

            foreach (var businessObjectInspectionAudit in businessObjectInspectionAudits)
            {
                await businessObjectInspectionAuditManager.DeleteAsync(businessObjectInspectionAudit);
            }
        }

        public async Task HandleAsync(EventHandlerContext context, BusinessObjectDeletionEvent @event)
        {
            var businessObjectInspectionAuditManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditManager>();

            var businessObjectInspectionAudits = businessObjectInspectionAuditManager
                .GetQueryable()
                .Where(entity => entity.BusinessObject == @event.UniqueName)
                .AsEnumerable();

            foreach (var businessObjectInspectionAudit in businessObjectInspectionAudits)
            {
                await businessObjectInspectionAuditManager.DeleteAsync(businessObjectInspectionAudit);
            }
        }
    }
}