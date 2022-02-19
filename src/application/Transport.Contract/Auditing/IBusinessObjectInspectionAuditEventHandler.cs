using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditEventHandler :
        IEventHandler<BusinessObjectEvent>,
        IEventHandler<BusinessObjectDeletionEvent>,
        IEventHandler<BusinessObjectInspectionAuditEvent>,
        IEventHandler<InspectionEvent>,
        IEventHandler<InspectionDeletionEvent>
    {

    }
}