using ChristianSchulz.ObjectInspection.Application.Administration.Events;
using ChristianSchulz.ObjectInspection.Application.Auditing.Events;
using ChristianSchulz.ObjectInspection.Application.Inventory.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IInspectorEventHandler :
    IEventHandler<OrganizationUpdateEvent>,
    IEventHandler<BusinessObjectUpdateEvent>,
    IEventHandler<BusinessObjectDeletionEvent>,
    IEventHandler<BusinessObjectInspectorCreationEvent>,
    IEventHandler<BusinessObjectInspectorDeletionEvent>,
    IEventHandler<BusinessObjectInspectionAuditScheduleEvent>
{
}