using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Auditing.Continuations;
using Super.Paula.Shared.Orchestration;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public class BusinessObjectInspectionAuditRecordContinuationHandler : IBusinessObjectInspectionAuditRecordContinuationHandler
{
    public async Task HandleAsync(ContinuationHandlerContext context, CreateBusinessObjectInspectionAuditRecordContinuation continuation)
    {
        var businessObjectInspectionAuditRecordManager = context.Services.GetRequiredService<IBusinessObjectInspectionAuditRecordManager>();

        var businessObjectInspectionAuditRecord = await businessObjectInspectionAuditRecordManager.GetOrDefaultAsync(
                continuation.BusinessObject,
                continuation.Inspection,
                continuation.AuditDate,
                continuation.AuditTime);

        if (businessObjectInspectionAuditRecord != null)
        {
            businessObjectInspectionAuditRecord.InspectionDisplayName = continuation.InspectionDisplayName;
            businessObjectInspectionAuditRecord.Annotation = continuation.AuditAnnotation;
            businessObjectInspectionAuditRecord.Result = continuation.AuditResult;
            businessObjectInspectionAuditRecord.BusinessObjectDisplayName = continuation.BusinessObjectDisplayName;
            businessObjectInspectionAuditRecord.Inspector = continuation.AuditInspector;

            await businessObjectInspectionAuditRecordManager.UpdateAsync(businessObjectInspectionAuditRecord);
        }
        else
        {
            businessObjectInspectionAuditRecord = new BusinessObjectInspectionAuditRecord
            {
                Annotation = continuation.AuditAnnotation,
                AuditDate = continuation.AuditDate,
                AuditTime = continuation.AuditTime,
                BusinessObject = continuation.BusinessObject,
                BusinessObjectDisplayName = continuation.BusinessObjectDisplayName,
                Inspection = continuation.Inspection,
                InspectionDisplayName = continuation.InspectionDisplayName,
                Inspector = continuation.AuditInspector,
                Result = continuation.AuditResult
            };

            await businessObjectInspectionAuditRecordManager.InsertAsync(businessObjectInspectionAuditRecord);
        }
    }
}