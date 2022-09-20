using ChristianSchulz.ObjectInspection.Application.Guidelines.Events;
using ChristianSchulz.ObjectInspection.Application.Inventory.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectionAuditRecordEventHandler :
    IEventHandler<BusinessObjectUpdateEvent>,
    IEventHandler<BusinessObjectDeletionEvent>,
    IEventHandler<InspectionUpdateEvent>,
    IEventHandler<InspectionDeletionEvent>
{

}