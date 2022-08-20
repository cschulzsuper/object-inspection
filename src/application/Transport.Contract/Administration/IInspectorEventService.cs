using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public interface IInspectorEventService
{
    ValueTask CreateInspectorBusinessObjectEventAsync(Inspector inspector, InspectorBusinessObject inspectorBusinessObject, bool oldDelayed, bool oldPending);
}