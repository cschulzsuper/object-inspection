using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    [HubName(nameof(Notification))]
    public class NotificationHub : Hub
    {

    }
}
