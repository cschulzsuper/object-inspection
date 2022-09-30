using Microsoft.AspNetCore.SignalR;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Application.SignalR;

public class RadioUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var user = connection.User;

        if (!user.Claims.HasInspector() || 
            !user.Claims.HasOrganization())
        {
            return null;
        }

        var userId = $"{user.Claims.GetOrganization()}:{user.Claims.GetInspector()}";

        return userId;
    }
}
