using ChristianSchulz.ObjectInspection.Application.Administration.Continuation;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IIdentityInspectorContinuationHandler :
    IContinuationHandler<CreateIdentityInspectorContinuation>,
    IContinuationHandler<ActivateIdentityInspectorContinuation>,
    IContinuationHandler<DeactivateIdentityInspectorContinuation>,
    IContinuationHandler<DeleteIdentityInspectorContinuation>
{
}