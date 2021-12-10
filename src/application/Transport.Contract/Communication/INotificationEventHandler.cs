using Super.Paula.Application.Inventory.Events;

namespace Super.Paula.Application.Communication
{
    public interface INotificationEventHandler : 
        IEventHandler<BusinessObjectInspectorEvent>
    {
    }
}