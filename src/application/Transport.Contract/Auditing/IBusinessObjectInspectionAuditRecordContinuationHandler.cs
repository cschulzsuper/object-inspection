using Super.Paula.Application.Auditing.Continuations;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionAuditRecordContinuationHandler :
    IContinuationHandler<CreateBusinessObjectInspectionAuditRecordContinuation>
{

}