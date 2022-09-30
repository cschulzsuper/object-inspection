using ChristianSchulz.ObjectInspection.Application.Auditing.Continuations;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectionAuditRecordContinuationHandler :
    IContinuationHandler<CreateBusinessObjectInspectionAuditRecordContinuation>
{

}