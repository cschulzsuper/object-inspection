using Microsoft.AspNetCore.SignalR;
using Super.Paula.SignalR;

namespace Super.Paula.Application.Communication
{
    [HubName(nameof(Notification))]
    public class NotificationHub : Hub
    {
        
    }
}
