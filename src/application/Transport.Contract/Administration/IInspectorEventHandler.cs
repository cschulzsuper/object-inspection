using Super.Paula.Application.Administration.Events;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorEventHandler : 
        IEventHandler<OrganizationEvent>
    {
    }
}