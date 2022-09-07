using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Shared.Orchestration;

namespace Super.Paula.Application.Communication;

public interface INotificationEventHandler :
    IEventHandler<BusinessObjectInspectorCreationEvent>,
    IEventHandler<BusinessObjectInspectorDeletionEvent>,
    IEventHandler<InspectorBusinessObjectImmediacyDetectionEvent>,
    IEventHandler<InspectorBusinessObjectOverdueDetectionEvent>
{
}