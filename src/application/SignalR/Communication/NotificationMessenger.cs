using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public class NotificationMessenger : INotificationMessenger
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly AppState _appState;

        public NotificationMessenger(IHubContext<NotificationHub> hubContext, AppState appState)
        {
            _hubContext = hubContext;
            _appState = appState;
        }

        public async Task OnCreatedAsync(NotificationResponse response)
        {
            var userId = $"{_appState.CurrentOrganization}:{response.Inspector}";

            await _hubContext.Clients.User(userId).SendAsync("OnCreated", response);
        }
    }
}
