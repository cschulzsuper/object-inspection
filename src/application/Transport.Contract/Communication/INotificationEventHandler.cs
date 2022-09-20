using ChristianSchulz.ObjectInspection.Application.Administration.Events;
using ChristianSchulz.ObjectInspection.Application.Auditing.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;

namespace ChristianSchulz.ObjectInspection.Application.Communication;

public interface INotificationEventHandler :
    IEventHandler<BusinessObjectInspectorCreationEvent>,
    IEventHandler<BusinessObjectInspectorDeletionEvent>,
    IEventHandler<InspectorBusinessObjectImmediacyDetectionEvent>,
    IEventHandler<InspectorBusinessObjectOverdueDetectionEvent>
{
}