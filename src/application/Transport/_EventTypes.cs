using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _EventTypes
    {
        public static IEventTypeRegistry ConfigureTransport(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.ConfigureTransportAdministration();
            eventTypeRegistry.ConfigureTransportAuditing();
            eventTypeRegistry.ConfigureTransportGuidlines();
            eventTypeRegistry.ConfigureTransportInventory();

            return eventTypeRegistry;
        }

        private static IEventTypeRegistry ConfigureTransportAdministration(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.Register<OrganizationCreationEvent>();
            eventTypeRegistry.Register<OrganizationDeletionEvent>();
            eventTypeRegistry.Register<OrganizationUpdateEvent>();
            eventTypeRegistry.Register<InspectorBusinessObjectEvent>();

            return eventTypeRegistry;
        }

        private static IEventTypeRegistry ConfigureTransportAuditing(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.Register<BusinessObjectInspectionAuditScheduleEvent>();

            return eventTypeRegistry;
        }

        private static IEventTypeRegistry ConfigureTransportGuidlines(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.Register<InspectionDeletionEvent>();
            eventTypeRegistry.Register<InspectionEvent>();

            return eventTypeRegistry;
        }

        private static IEventTypeRegistry ConfigureTransportInventory(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.Register<BusinessObjectInspectorEvent>();
            eventTypeRegistry.Register<BusinessObjectEvent>();
            eventTypeRegistry.Register<BusinessObjectDeletionEvent>();

            return eventTypeRegistry;
        }
    }
}
