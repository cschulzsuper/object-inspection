using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionEventHandler :
    IEventHandler<InspectionEvent>,
    IEventHandler<InspectionDeletionEvent>
{
}