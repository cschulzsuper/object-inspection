using Super.Paula.Application.Administration;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Operation;
using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Events
    {
        public static IEventBus ConfigureTransport(this IEventBus eventBus)
        {
            eventBus.ConfigureTransportAdministration();
            eventBus.ConfigureTransportAuditing();
            eventBus.ConfigureTransportInventory();
            eventBus.ConfigureTransportOperation();
            eventBus.ConfigureTransportCommunication();

            return eventBus;
        }

        private static IEventBus ConfigureTransportAdministration(this IEventBus eventBus)
        {
            eventBus.Subscribe<OrganizationUpdateEvent, InspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);

            eventBus.Subscribe<BusinessObjectDeletionEvent, InspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);

            eventBus.Subscribe<BusinessObjectInspectorEvent, InspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);

            eventBus.Subscribe<BusinessObjectInspectionAuditScheduleEvent, InspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);

            return eventBus;
        }

        private static IEventBus ConfigureTransportAuditing(this IEventBus eventBus)
        {

            eventBus.Subscribe<BusinessObjectEvent, BusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<BusinessObjectDeletionEvent, BusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<BusinessObjectInspectionAuditEvent, BusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<InspectionEvent, BusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<InspectionDeletionEvent, BusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            return eventBus;
        }

        private static IEventBus ConfigureTransportCommunication(this IEventBus eventBus)
        {

            eventBus.Subscribe<BusinessObjectInspectorEvent, NotificationEventHandler>(
                AllowedSubscribers.CommunicationNotification);

            eventBus.Subscribe<InspectorBusinessObjectEvent, NotificationEventHandler>(
                AllowedSubscribers.CommunicationNotification);

            return eventBus;
        }

        private static IEventBus ConfigureTransportInventory(this IEventBus eventBus)
        {

            eventBus.Subscribe<InspectionEvent, BusinessObjectEventHandler>(
                AllowedSubscribers.InventoryBusinessObject);

            eventBus.Subscribe<InspectionDeletionEvent, BusinessObjectEventHandler>(
                AllowedSubscribers.InventoryBusinessObject);

            return eventBus;
        }

        private static IEventBus ConfigureTransportOperation(this IEventBus eventBus)
        {

            eventBus.Subscribe<OrganizationCreationEvent, ApplicationEventHandler>(
                AllowedSubscribers.OperationApplication);

            eventBus.Subscribe<OrganizationDeletionEvent, ApplicationEventHandler>(
                AllowedSubscribers.OperationApplication);

            return eventBus;
        }
    }
}
