using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Continuation;
using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Continuations
    {
        public static IContinuator ConfigureTransport(this IContinuator continuator)
        {
            continuator.ConfigureTransportAdministration();

            return continuator;
        }

        private static IContinuator ConfigureTransportAdministration(this IContinuator continuator)
        {
            continuator.Register<CreateInspectorContinuation, InspectorContinuationHandler>();

            continuator.Register<CreateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();
            continuator.Register<DeleteIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();
            continuator.Register<ActivateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();
            continuator.Register<DeactivateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();

            return continuator;
        }
    }
}
