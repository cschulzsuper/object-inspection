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
            eventBus.Subscribe<OrganizationUpdateEvent, IInspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);

            eventBus.Subscribe<BusinessObjectDeletionEvent, IInspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);

            eventBus.Subscribe<BusinessObjectInspectorEvent, IInspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);

            eventBus.Subscribe<BusinessObjectInspectionAuditScheduleEvent, IInspectorEventHandler>(
                AllowedSubscribers.AdministrationInspector);


            eventBus.Subscribe<BusinessObjectEvent, IBusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<BusinessObjectDeletionEvent, IBusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<BusinessObjectInspectionAuditEvent, IBusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<InspectionEvent, IBusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);

            eventBus.Subscribe<InspectionDeletionEvent, IBusinessObjectInspectionAuditEventHandler>(
                AllowedSubscribers.AuditingBusinessObjectInspectionAudit);


            eventBus.Subscribe<BusinessObjectInspectorEvent, INotificationEventHandler>(
                AllowedSubscribers.CommunicationNotification);

            eventBus.Subscribe<InspectorBusinessObjectEvent, INotificationEventHandler>(
                AllowedSubscribers.CommunicationNotification);


            eventBus.Subscribe<InspectionEvent, IBusinessObjectEventHandler>(
                AllowedSubscribers.InventoryBusinessObject);

            eventBus.Subscribe<InspectionDeletionEvent, IBusinessObjectEventHandler>(
                AllowedSubscribers.InventoryBusinessObject);


            eventBus.Subscribe<OrganizationCreationEvent, IApplicationEventHandler>(
                AllowedSubscribers.OperationApplication);

            eventBus.Subscribe<OrganizationDeletionEvent, IApplicationEventHandler>(
                AllowedSubscribers.OperationApplication);

            return eventBus;
        }
    }
}
