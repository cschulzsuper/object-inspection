using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Administration;

public interface IInspectorEventHandler :
    IEventHandler<OrganizationUpdateEvent>,
    IEventHandler<BusinessObjectUpdateEvent>,
    IEventHandler<BusinessObjectDeletionEvent>,
    IEventHandler<BusinessObjectInspectorCreationEvent>,
    IEventHandler<BusinessObjectInspectorDeletionEvent>,
    IEventHandler<BusinessObjectInspectionAuditScheduleEvent>
{
}