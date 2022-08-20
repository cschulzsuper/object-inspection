using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionAuditRecordEventHandler :
    IEventHandler<BusinessObjectEvent>,
    IEventHandler<BusinessObjectDeletionEvent>,
    IEventHandler<InspectionEvent>,
    IEventHandler<InspectionDeletionEvent>
{

}