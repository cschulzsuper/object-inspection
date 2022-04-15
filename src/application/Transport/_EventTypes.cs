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
            eventTypeRegistry.Register<OrganizationCreationEvent>("event-organization-creation");
            eventTypeRegistry.Register<OrganizationDeletionEvent>("event-organization-deletion");
            eventTypeRegistry.Register<OrganizationUpdateEvent>("event-organization-update");
            eventTypeRegistry.Register<InspectorBusinessObjectEvent>("event-inspector-business-object");

            return eventTypeRegistry;
        }

        private static IEventTypeRegistry ConfigureTransportAuditing(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.Register<BusinessObjectInspectionAuditScheduleEvent>("event-business-object-inspection-audit-schedule");

            return eventTypeRegistry;
        }

        private static IEventTypeRegistry ConfigureTransportGuidlines(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.Register<InspectionDeletionEvent>("event-inspection-deletion");
            eventTypeRegistry.Register<InspectionEvent>("event-inspection");

            return eventTypeRegistry;
        }

        private static IEventTypeRegistry ConfigureTransportInventory(this IEventTypeRegistry eventTypeRegistry)
        {
            eventTypeRegistry.Register<BusinessObjectInspectorEvent>("event-business-object-inspector");
            eventTypeRegistry.Register<BusinessObjectEvent>("event-business-object");
            eventTypeRegistry.Register<BusinessObjectDeletionEvent>("event-business-object-deletion");

            return eventTypeRegistry;
        }
    }
}
