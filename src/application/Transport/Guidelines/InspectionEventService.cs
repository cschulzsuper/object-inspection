using Super.Paula.Application.Guidelines.Events;
using Super.Paula.Shared.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Guidelines;

public class InspectionEventService : IInspectionEventService
{
    private readonly IEventStorage _eventStorage;
    private readonly ClaimsPrincipal _user;

    public InspectionEventService(
        IEventStorage eventStorage,
        ClaimsPrincipal user)
    {
        _eventStorage = eventStorage;
        _user = user;
    }

    public async ValueTask CreateInspectionUpdateEventAsync(Inspection inspection)
    {
        var @event = new InspectionUpdateEvent(
            inspection.UniqueName,
            inspection.DisplayName,
            inspection.Text,
            inspection.Activated);

        await _eventStorage.AddAsync(@event, _user);
    }

    public async ValueTask CreateInspectionDeletionEventAsync(string inspection)
    {
        var @event = new InspectionDeletionEvent(inspection);

        await _eventStorage.AddAsync(@event, _user);
    }
}