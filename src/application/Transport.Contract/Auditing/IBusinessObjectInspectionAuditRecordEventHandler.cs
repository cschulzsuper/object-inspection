using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionAuditRecordEventHandler :
    IEventHandler<BusinessObjectUpdateEvent>,
    IEventHandler<BusinessObjectDeletionEvent>,
    IEventHandler<InspectionUpdateEvent>,
    IEventHandler<InspectionDeletionEvent>
{

}