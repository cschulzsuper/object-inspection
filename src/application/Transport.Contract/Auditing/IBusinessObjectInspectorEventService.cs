using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectorEventService
{
    ValueTask CreateBusinessObjectInspectorCreationEventAsync(BusinessObjectInspector businessObjectInspector);
    ValueTask CreateBusinessObjectInspectorDeletionEventAsync(BusinessObjectInspector businessObjectInspector);
}