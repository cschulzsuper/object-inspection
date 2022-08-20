using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Continuation;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auditing.Continuations;
using Super.Paula.Shared.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application;

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
        continuationRegistry.Register<CreateInspectorContinuation, InspectorContinuationHandler>();

        continuationRegistry.Register<CreateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();
        continuationRegistry.Register<DeleteIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();
        continuationRegistry.Register<ActivateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();
        continuationRegistry.Register<DeactivateIdentityInspectorContinuation, IdentityInspectorContinuationHandler>();

        return continuationRegistry;
    }

    private static IContinuationRegistry ConfigureTransportAuditing(this IContinuationRegistry continuationRegistry)
    {
        continuationRegistry.Register<CreateBusinessObjectInspectionAuditRecordContinuation, BusinessObjectInspectionAuditRecordContinuationHandler>();

        return continuationRegistry;
    }
}