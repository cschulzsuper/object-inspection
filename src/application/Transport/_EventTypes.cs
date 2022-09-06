using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Shared.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application;

[SuppressMessage("Style", "IDE1006")]
public static class _EventTypes
{
    public static IEventTypeRegistry ConfigureTransport(this IEventTypeRegistry eventTypeRegistry)
    {
        eventTypeRegistry.ConfigureTransportAdministration();
        eventTypeRegistry.ConfigureTransportAuditing();
        eventTypeRegistry.ConfigureTransportGuidelines();
        eventTypeRegistry.ConfigureTransportInventory();

        return eventTypeRegistry;
    }

    private static IEventTypeRegistry ConfigureTransportAdministration(this IEventTypeRegistry eventTypeRegistry)
    {
        eventTypeRegistry.Register<OrganizationCreationEvent>();
        eventTypeRegistry.Register<OrganizationDeletionEvent>();
        eventTypeRegistry.Register<OrganizationUpdateEvent>();
        eventTypeRegistry.Register<InspectorBusinessObjectOverdueDetectionEvent>();
        eventTypeRegistry.Register<InspectorBusinessObjectImmediacyDetectionEvent>();

        return eventTypeRegistry;
    }

    private static IEventTypeRegistry ConfigureTransportAuditing(this IEventTypeRegistry eventTypeRegistry)
    {
        eventTypeRegistry.Register<BusinessObjectInspectorCreationEvent>();
        eventTypeRegistry.Register<BusinessObjectInspectorDeletionEvent>();
        eventTypeRegistry.Register<BusinessObjectInspectionAuditScheduleEvent>();

        return eventTypeRegistry;
    }

    private static IEventTypeRegistry ConfigureTransportGuidelines(this IEventTypeRegistry eventTypeRegistry)
    {
        eventTypeRegistry.Register<InspectionDeletionEvent>();
        eventTypeRegistry.Register<InspectionUpdateEvent>();

        return eventTypeRegistry;
    }

    private static IEventTypeRegistry ConfigureTransportInventory(this IEventTypeRegistry eventTypeRegistry)
    {
        eventTypeRegistry.Register<BusinessObjectInspectorCreationEvent>();
        eventTypeRegistry.Register<BusinessObjectInspectorDeletionEvent>();
        eventTypeRegistry.Register<BusinessObjectUpdateEvent>();
        eventTypeRegistry.Register<BusinessObjectDeletionEvent>();

        return eventTypeRegistry;
    }
}