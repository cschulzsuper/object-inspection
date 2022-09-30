using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Administration.Continuation;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using ChristianSchulz.ObjectInspection.Application.Auditing.Continuations;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace ChristianSchulz.ObjectInspection.Application;

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