using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Administration;

public interface IInspectorEventHandler :
    IEventHandler<OrganizationUpdateEvent>,
    IEventHandler<BusinessObjectDeletionEvent>,
    IEventHandler<BusinessObjectInspectorEvent>,
    IEventHandler<BusinessObjectInspectionAuditScheduleEvent>
{
}