using Super.Paula.Application.Administration.Events;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Operation;

public interface IApplicationEventHandler :
    IEventHandler<OrganizationCreationEvent>,
    IEventHandler<OrganizationDeletionEvent>
{
}