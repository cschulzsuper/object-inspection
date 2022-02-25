using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class InspectorEventService : IInspectorEventService
    {
        private readonly IEventStorage _eventStorage;
        private readonly ClaimsPrincipal _user;

        public InspectorEventService(
            IEventStorage eventStorage,
            ClaimsPrincipal user)
        {
            _eventStorage = eventStorage;
            _user = user;
        }

        public async ValueTask CreateInspectorBusinessObjectEventAsync(
            Inspector inspector,
            InspectorBusinessObject inspectorBusinessObject,
            bool oldDelayed,
            bool oldPending)

        {
            var @event = new InspectorBusinessObjectEvent(
                inspector.UniqueName,
                inspectorBusinessObject.UniqueName,
                inspectorBusinessObject.DisplayName,
                inspectorBusinessObject.AuditScheduleDelayed,
                inspectorBusinessObject.AuditSchedulePending,
                oldDelayed,
                oldPending);

            await _eventStorage.AddAsync(@event, _user);
        }
    }
}