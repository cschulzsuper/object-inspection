using Super.Paula.Application.Auditing;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectEventService : IBusinessObjectEventService
    {
        private readonly ClaimsPrincipal _user;
        private readonly IEventStorage _eventStorage;

        public BusinessObjectEventService(
            ClaimsPrincipal user,
            IEventStorage eventStorage)
        {
            _user = user;
            _eventStorage = eventStorage;
        }

        public async ValueTask CreateBusinessObjectEventAsync(BusinessObject businessObject)
        {
            var @event = new BusinessObjectEvent(
                businessObject.UniqueName,
                businessObject.DisplayName);

            await _eventStorage.AddAsync(@event, _user);
        }

        public async ValueTask CreateBusinessObjectDeletionEventAsync(string businessObject)
        {
            var @event = new BusinessObjectDeletionEvent(businessObject);

            await _eventStorage.AddAsync(@event, _user);
        }

        public async ValueTask CreateBusinessObjectInspectorEventAsync(BusinessObject businessObject, string newInspector, string oldInspector)
        {
            var @event = new BusinessObjectInspectorEvent(
                businessObject.UniqueName,
                businessObject.DisplayName,
                newInspector,
                oldInspector);

            await _eventStorage.AddAsync(@event, _user);
        }
    }
}