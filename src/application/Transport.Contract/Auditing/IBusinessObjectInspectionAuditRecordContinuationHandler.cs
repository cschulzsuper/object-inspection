using Super.Paula.Application.Auditing.Continuations;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditRecordContinuationHandler :
        IContinuationHandler<CreateBusinessObjectInspectionAuditRecordContinuation>
    {

    }
}