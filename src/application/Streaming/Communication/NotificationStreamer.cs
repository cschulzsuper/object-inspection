using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Application.Streaming;
using Super.Paula.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public sealed class NotificationStreamer : INotificationStreamer, IDisposable
    {
        private readonly IHubContext _streamHubContext;
        private readonly ClaimsPrincipal _claimsPrincipal;

        private bool _disposed;

        public NotificationStreamer(HubContextResolver hubContextResolver, ClaimsPrincipal claimsPrincipal)
        {
            _streamHubContext = hubContextResolver.GetHubContext("Stream");
            _claimsPrincipal = claimsPrincipal;
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

            var userId = $"{_claimsPrincipal.GetOrganization()}:{response.Inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("NotificationCreation", response);
        }

        public async Task StreamNotificationDeletionAsync(string inspector, int date, int time)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_claimsPrincipal.GetOrganization()}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("NotificationDeletion", inspector, date, time);
        }
    }
}
