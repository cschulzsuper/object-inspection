using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Administration.Events;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using ChristianSchulz.ObjectInspection.Application.Auditing.Events;
using ChristianSchulz.ObjectInspection.Application.Communication;
using ChristianSchulz.ObjectInspection.Application.Guidelines.Events;
using ChristianSchulz.ObjectInspection.Application.Inventory.Events;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace ChristianSchulz.ObjectInspection.Application;

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

        eventBus.Subscribe<BusinessObjectUpdateEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        eventBus.Subscribe<BusinessObjectDeletionEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        eventBus.Subscribe<BusinessObjectInspectorCreationEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        eventBus.Subscribe<BusinessObjectInspectorDeletionEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        eventBus.Subscribe<BusinessObjectInspectionAuditScheduleEvent, InspectorEventHandler>(
            AllowedSubscribers.Inspector);

        return eventBus;
    }

    private static IEventBus ConfigureTransportAuditing(this IEventBus eventBus)
    {
        eventBus.Subscribe<InspectionUpdateEvent, BusinessObjectInspectionEventHandler>(
            AllowedSubscribers.BusinessObjectInspection);

        eventBus.Subscribe<InspectionDeletionEvent, BusinessObjectInspectionEventHandler>(
            AllowedSubscribers.BusinessObjectInspection);

        eventBus.Subscribe<BusinessObjectUpdateEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        eventBus.Subscribe<BusinessObjectDeletionEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        eventBus.Subscribe<InspectionUpdateEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        eventBus.Subscribe<InspectionDeletionEvent, BusinessObjectInspectionAuditRecordEventHandler>(
            AllowedSubscribers.BusinessObjectInspectionAuditRecord);

        return eventBus;
    }

    private static IEventBus ConfigureTransportCommunication(this IEventBus eventBus)
    {

        eventBus.Subscribe<BusinessObjectInspectorCreationEvent, NotificationEventHandler>(
            AllowedSubscribers.Notification);

        eventBus.Subscribe<BusinessObjectInspectorDeletionEvent, NotificationEventHandler>(
            AllowedSubscribers.Notification);

        eventBus.Subscribe<InspectorBusinessObjectImmediacyDetectionEvent, NotificationEventHandler>(
            AllowedSubscribers.Notification);

        eventBus.Subscribe<InspectorBusinessObjectOverdueDetectionEvent, NotificationEventHandler>(
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