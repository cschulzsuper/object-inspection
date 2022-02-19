using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorEventHandler :
        IEventHandler<OrganizationEvent>,
        IEventHandler<BusinessObjectDeletionEvent>,
        IEventHandler<BusinessObjectInspectorEvent>,
        IEventHandler<BusinessObjectInspectionAuditScheduleEvent>
    {
    }
}