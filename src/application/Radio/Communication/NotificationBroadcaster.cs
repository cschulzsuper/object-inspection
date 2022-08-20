using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Shared.Authorization;
using Super.Paula.Shared.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication;

public sealed class NotificationBroadcaster : INotificationBroadcaster, IDisposable
{
    private readonly IHubContext _radioHubContext;
    private readonly ClaimsPrincipal _claimsPrincipal;

    private bool _disposed;

    public NotificationBroadcaster(HubContextResolver hubContextResolver, ClaimsPrincipal claimsPrincipal)
    {
        _radioHubContext = hubContextResolver.GetHubContext("Radio");
        _claimsPrincipal = claimsPrincipal;
    }

    public void Dispose()
    {
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async Task SendNotificationCreationAsync(NotificationResponse response)
    {
        if (_disposed)
        {
            return;
        }

        var userId = $"{_claimsPrincipal.GetOrganization()}:{response.Inspector}";
        await _radioHubContext.Clients.User(userId).SendAsync("NotificationCreation", response);
    }

    public async Task SendNotificationDeletionAsync(string inspector, int date, int time)
    {
        if (_disposed)
        {
            return;
        }

        var userId = $"{_claimsPrincipal.GetOrganization()}:{inspector}";
        await _radioHubContext.Clients.User(userId).SendAsync("NotificationDeletion", inspector, date, time);
    }
}