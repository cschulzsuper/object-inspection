using Super.Paula.Application.Administration.Continuation;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorContinuationHandler :
        IContinuationHandler<CreateInspectorContinuation>
    {
    }
}