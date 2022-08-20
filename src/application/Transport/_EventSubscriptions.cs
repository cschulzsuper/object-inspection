using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Operation;
using Super.Paula.Shared.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application;

[SuppressMessage("Style", "IDE1006")]
public static class _EventSubscriptions
{
    public static IEventBus ConfigureTransport(this IEventBus eventBus)
    {
        eventBus.ConfigureTransportAdministration();
        eventBus.ConfigureTransportAuditing();
        eventBus.ConfigureTransportOperation();
        eventBus.ConfigureTransportCommunication();

        return eventBus;
    }

    private static IEventBus ConfigureTransportAdministration(this IEventBus eventBus)
    {
        eventBus.Subscribe<OrganizationUpdateEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        eventBus.Subscribe<BusinessObjectDeletionEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        eventBus.Subscribe<BusinessObjectInspectorEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        eventBus.Subscribe<BusinessObjectInspectionAuditScheduleEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        return eventBus;
    }

    private static IEventBus ConfigureTransportAuditing(this IEventBus eventBus)
    {
        eventBus.Subscribe<InspectionEvent, BusinessObjectInspectionEventHandler>(
            AllowedSubscribers.BusinessObjectInspection);

        eventBus.Subscribe<InspectionDeletionEvent, BusinessObjectInspectionEventHandler>(
            AllowedSubscribers.BusinessObjectInspection);

        eventBus.Subscribe<BusinessObjectEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        eventBus.Subscribe<BusinessObjectDeletionEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        eventBus.Subscribe<InspectionEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        eventBus.Subscribe<InspectionDeletionEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        return eventBus;
    }

    private static IEventBus ConfigureTransportCommunication(this IEventBus eventBus)
    {

        eventBus.Subscribe<BusinessObjectInspectorEvent, NotificationEventHandler>(
            AllowedSubscribers.Notification);

        eventBus.Subscribe<InspectorBusinessObjectEvent, NotificationEventHandler>(
            AllowedSubscribers.Notification);

        return eventBus;
    }

    private static IEventBus ConfigureTransportOperation(this IEventBus eventBus)
    {

        eventBus.Subscribe<OrganizationCreationEvent, ApplicationEventHandler>(
            AllowedSubscribers.Application);

        eventBus.Subscribe<OrganizationDeletionEvent, ApplicationEventHandler>(
            AllowedSubscribers.Application);

        return eventBus;
    }
}