using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Inventory;

public interface IBusinessObjectEventService
{
    ValueTask CreateBusinessObjectUpdateEventAsync(BusinessObject businessObject);
    ValueTask CreateBusinessObjectDeletionEventAsync(string businessObject);
}