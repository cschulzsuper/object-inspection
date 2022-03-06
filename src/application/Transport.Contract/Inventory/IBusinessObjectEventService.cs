using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public interface IBusinessObjectEventService
    {
        ValueTask CreateBusinessObjectEventAsync(BusinessObject businessObject);
        ValueTask CreateBusinessObjectDeletionEventAsync(string businessObject);
        ValueTask CreateBusinessObjectInspectorEventAsync(BusinessObject businessObject, string newInspector, string oldInspector);
    }
}