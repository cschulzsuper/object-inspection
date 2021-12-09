using Super.Paula.Application.Guidelines.Events;

namespace Super.Paula.Application.Inventory
{
    public interface IBusinessObjectEventHandler : 
        IEventHandler<InspectionEvent>
    {
    }
}