using ChristianSchulz.ObjectInspection.Application.Auditing.Continuations;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspectionContinuationService : IBusinessObjectInspectionContinuationService
{
    private readonly ClaimsPrincipal _user;
    private readonly IContinuationStorage _continuationStorage;

    public BusinessObjectInspectionContinuationService(
        ClaimsPrincipal user,
        IContinuationStorage continuationStorage)
    {
        _user = user;
        _continuationStorage = continuationStorage;
    }

    public async ValueTask AddCreateBusinessObjectInspectionAuditRecordContinuationAsync(BusinessObjectInspection inspection)
    {
        if (inspection.Audit.AuditDate == default)
        {
            return;
        }

        var continuation = new CreateBusinessObjectInspectionAuditRecordContinuation(
            inspection.BusinessObject,
            inspection.BusinessObjectDisplayName,
            inspection.Audit.Inspector,
            inspection.Inspection,
            inspection.InspectionDisplayName,
            inspection.Audit.Annotation,
            inspection.Audit.Result,
            inspection.Audit.AuditDate,
            inspection.Audit.AuditTime);

        await _continuationStorage.AddAsync(continuation, _user);
    }
}