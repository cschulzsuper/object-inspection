using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Environment;
using System.Threading.Tasks;

namespace Super.Paula.Application.Streaming
{
    public class Streamer : IStreamer
    {
        private readonly IHubContext _streamHubContext;
        private readonly AppState _appState;

        public Streamer(HubContextResolver hubContextResolver, AppState appState)
        {

            _streamHubContext = hubContextResolver.GetHubContext("Stream");
            _appState = appState;
        }

        public async Task StreamNotificationCreationAsync(NotificationResponse response)
        {
            var userId = $"{_appState.CurrentOrganization}:{response.Inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("NotificationCreation", response);
        }

        public async Task StreamNotificationDeletionAsync(string inspector, int date, int time)
        {
            var userId = $"{_appState.CurrentOrganization}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("NotificationDeletion", inspector, date, time);
        }
    }
}
