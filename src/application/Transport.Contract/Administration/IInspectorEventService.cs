using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IInspectorEventService
{
    ValueTask CreateInspectorBusinessObjectImmediacyDetectionEventAsync(Inspector inspector, InspectorBusinessObject inspectorBusinessObject);

    ValueTask CreateInspectorBusinessObjectOverdueDetectionEventAsync(Inspector inspector, InspectorBusinessObject inspectorBusinessObject);
}