using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionEventHandler :
        IEventHandler<InspectionEvent>,
        IEventHandler<InspectionDeletionEvent>
    {
    }
}