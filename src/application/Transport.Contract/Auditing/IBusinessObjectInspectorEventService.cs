using Super.Paula.Application.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectorEventService
{
    ValueTask CreateBusinessObjectInspectorCreationEventAsync(BusinessObjectInspector businessObjectInspector);
    ValueTask CreateBusinessObjectInspectorDeletionEventAsync(BusinessObjectInspector businessObjectInspector);
}