using Super.Paula.Application.Inventory.Events;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditEventHandler :
        IEventHandler<BusinessObjectEvent>,
        IEventHandler<BusinessObjectInspectionEvent>
    {

    }
}