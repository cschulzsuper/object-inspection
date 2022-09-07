using Super.Paula.Shared.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Inventory.Events;

namespace Super.Paula.Application.Auditing;

public class BusinessObjectInspectorEventService : IBusinessObjectInspectorEventService
{
    private readonly ClaimsPrincipal _user;
    private readonly IEventStorage _eventStorage;

    public BusinessObjectInspectorEventService(
        ClaimsPrincipal user,
        IEventStorage eventStorage)
    {
        _user = user;
        _eventStorage = eventStorage;
    }

    public async ValueTask CreateBusinessObjectInspectorCreationEventAsync(BusinessObjectInspector businessObjectInspector)
    {
        var @event = new BusinessObjectInspectorCreationEvent(
            businessObjectInspector.BusinessObject,
            businessObjectInspector.BusinessObjectDisplayName,
            businessObjectInspector.Inspector);

        await _eventStorage.AddAsync(@event, _user);
    }

    public async ValueTask CreateBusinessObjectInspectorDeletionEventAsync(BusinessObjectInspector businessObjectInspector)
    {
        var @event = new BusinessObjectInspectorDeletionEvent(
            businessObjectInspector.BusinessObject,
            businessObjectInspector.BusinessObjectDisplayName,
            businessObjectInspector.Inspector);

        await _eventStorage.AddAsync(@event, _user);
    }
}