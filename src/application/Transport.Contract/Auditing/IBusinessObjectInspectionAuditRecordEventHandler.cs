using Super.Paula.Application.Auditing.Continuations;
using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditRecordEventHandler :
        IEventHandler<BusinessObjectEvent>,
        IEventHandler<BusinessObjectDeletionEvent>,
        IEventHandler<InspectionEvent>,
        IEventHandler<InspectionDeletionEvent>
    {

    }
}