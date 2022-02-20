using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Application.Inventory
{
    public interface IBusinessObjectEventHandler :
        IEventHandler<InspectionEvent>,
        IEventHandler<InspectionDeletionEvent>
    {
    }
}