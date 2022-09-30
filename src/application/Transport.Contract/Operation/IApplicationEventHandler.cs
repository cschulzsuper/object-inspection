using ChristianSchulz.ObjectInspection.Application.Administration.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IApplicationEventHandler :
    IEventHandler<OrganizationCreationEvent>,
    IEventHandler<OrganizationDeletionEvent>
{
}