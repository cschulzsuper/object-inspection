using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Environment;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Streaming
{
    public sealed class Streamer : IStreamer, IDisposable
    {
        private readonly IHubContext _streamHubContext;
        private readonly AppState _appState;
        
        private bool _disposed;

        public Streamer(HubContextResolver hubContextResolver, AppState appState)
        {

            _streamHubContext = hubContextResolver.GetHubContext("Stream");
            _appState = appState;
        }

        public void Dispose()
        {
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public async Task StreamNotificationCreationAsync(NotificationResponse response)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_appState.CurrentOrganization}:{response.Inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("NotificationCreation", response);
        }

        public async Task StreamNotificationDeletionAsync(string inspector, int date, int time)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_appState.CurrentOrganization}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("NotificationDeletion", inspector, date, time);
        }

        public async Task StreamInspectorBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_appState.CurrentOrganization}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectCreation", inspector, response);
        }

        public async Task StreamInspectorBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_appState.CurrentOrganization}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectUpdate", inspector, response);
        }

        public async Task StreamInspectorBusinessObjectDeletionAsync(string inspector, string businessObject)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_appState.CurrentOrganization}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectDeletion", inspector, businessObject);
        }
    }
}
