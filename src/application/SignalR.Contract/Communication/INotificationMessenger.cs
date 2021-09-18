using Super.Paula.Application.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public interface INotificationMessenger
    {
        Task OnCreatedAsync(NotificationResponse response);
    }
}
