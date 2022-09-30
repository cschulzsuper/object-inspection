using Microsoft.AspNetCore.SignalR;
using ChristianSchulz.ObjectInspection.Application.Communication.Responses;
using ChristianSchulz.ObjectInspection.Shared.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Application.Communication;

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

        var userId = $"{_claimsPrincipal.Claims.GetOrganization()}:{response.Inspector}";
        await _radioHubContext.Clients.User(userId).SendAsync("NotificationCreation", response);
    }

    public async Task SendNotificationDeletionAsync(string inspector, int date, int time)
    {
        if (_disposed)
        {
            return;
        }

        var userId = $"{_claimsPrincipal.Claims.GetOrganization()}:{inspector}";
        await _radioHubContext.Clients.User(userId).SendAsync("NotificationDeletion", inspector, date, time);
    }
}