using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Administration.Responses;
using Super.Paula.Application.Streaming;
using Super.Paula.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public sealed class InspectorStreamer : IInspectorStreamer, IDisposable
    {
        private readonly IHubContext _streamHubContext;
        private readonly ClaimsPrincipal _claimsPrincipal;

        private bool _disposed;

        public InspectorStreamer(HubContextResolver hubContextResolver, ClaimsPrincipal claimsPrincipal)
        {

            _streamHubContext = hubContextResolver.GetHubContext("Stream");
            _claimsPrincipal = claimsPrincipal;
        }

        public void Dispose()
        {
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public async Task StreamInspectorBusinessObjectCreationAsync(string inspector, InspectorBusinessObjectResponse response)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_claimsPrincipal.GetOrganization()}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectCreation", inspector, response);
        }

        public async Task StreamInspectorBusinessObjectUpdateAsync(string inspector, InspectorBusinessObjectResponse response)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_claimsPrincipal.GetOrganization()}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectUpdate", inspector, response);
        }

        public async Task StreamInspectorBusinessObjectDeletionAsync(string inspector, string businessObject)
        {
            if (_disposed)
            {
                return;
            }

            var userId = $"{_claimsPrincipal.GetOrganization()}:{inspector}";
            await _streamHubContext.Clients.User(userId).SendAsync("InspectorBusinessObjectDeletion", inspector, businessObject);
        }
    }
}
