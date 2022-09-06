using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory;

public interface IBusinessObjectEventService
{
    ValueTask CreateBusinessObjectUpdateEventAsync(BusinessObject businessObject);
    ValueTask CreateBusinessObjectDeletionEventAsync(string businessObject);
    ValueTask CreateBusinessObjectInspectorCreationEventAsync(BusinessObject businessObject);
    ValueTask CreateBusinessObjectInspectorDeletionEventAsync(BusinessObject businessObject, string inspector);
}