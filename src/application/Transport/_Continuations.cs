using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Continuation;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auditing.Continuations;
using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Continuations
    {
        public static IContinuationRegistry ConfigureTransport(this IContinuationRegistry continuationRegistry)
        {
            continuationRegistry.ConfigureTransportAdministration();
            continuationRegistry.ConfigureTransportAuditing();

            return continuationRegistry;
        }

        private static IContinuationRegistry ConfigureTransportAdministration(this IContinuationRegistry continuationRegistry)
        {
            continuationRegistry.Register<CreateInspectorContinuation, InspectorContinuationHandler>("create-inspector");

            continuationRegistry.Register<CreateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>("create-identity-inspector");
            continuationRegistry.Register<DeleteIdentityInspectorContinuation, IdentityInspectorContinuationHandler>("delete-identity-inspector");
            continuationRegistry.Register<ActivateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>("activate-identity-inspector");
            continuationRegistry.Register<DeactivateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>("deactivate-identity-inspector");

            return continuationRegistry;
        }

        private static IContinuationRegistry ConfigureTransportAuditing(this IContinuationRegistry continuationRegistry)
        {
            continuationRegistry.Register<CreateBusinessObjectInspectionAuditRecordContinuation, BusinessObjectInspectionAuditRecordContinuationHandler>("create-business-object-inspection-audit-record");

            return continuationRegistry;
        }
    }
}
