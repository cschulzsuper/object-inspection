using ChristianSchulz.ObjectInspection.Application.Guidelines.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectionEventHandler :
    IEventHandler<InspectionUpdateEvent>,
    IEventHandler<InspectionDeletionEvent>
{
}