using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Environment;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public class NotificationMessenger : INotificationMessenger
    {
        private readonly IHubContext _hubContext;
        private readonly AppState _appState;

        public NotificationMessenger(HubContextResolver hubContextResolver, AppState appState)
        {

            _hubContext = hubContextResolver.GetHubContext(nameof(Notification));
            _appState = appState;
        }

        public async Task OnCreatedAsync(NotificationResponse response)
        {
            var userId = $"{_appState.CurrentOrganization}:{response.Inspector}";
            await _hubContext.Clients.User(userId).SendAsync("OnCreated", response);
        }

        public async Task OnDeletedAsync(string inspector, int date, int time)
        {
            var userId = $"{_appState.CurrentOrganization}:{inspector}";
            await _hubContext.Clients.User(userId).SendAsync("OnDeleted", inspector, date, time);
        }
    }
}
