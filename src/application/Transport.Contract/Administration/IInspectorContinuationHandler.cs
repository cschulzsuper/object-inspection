using ChristianSchulz.ObjectInspection.Application.Administration.Continuation;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IInspectorContinuationHandler :
    IContinuationHandler<CreateInspectorContinuation>
{
}