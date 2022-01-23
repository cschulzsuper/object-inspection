using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Inventory.Events;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorEventHandler : 
        IEventHandler<OrganizationEvent>,
        IEventHandler<BusinessObjectInspectorEvent>,
        IEventHandler<BusinessObjectInspectionAuditScheduleEvent>
    {
    }
}