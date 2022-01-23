using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Super.Paula.Application.Streaming
{
    [HubName("Stream")]
    public class StreamHub : Hub
    {
        [Authorize("Streamer")]
        public Task Stream(string userId, string method)
            => Clients.User(userId).SendAsync(method);

        [Authorize("Streamer")]
        public Task Stream1(string userId, string method, object? arg1)
            => Clients.User(userId).SendAsync(method, arg1);

        [Authorize("Streamer")]
        public Task Stream2(string userId, string method, object? arg1, object? arg2)
            => Clients.User(userId).SendAsync(method, arg1, arg2);

        [Authorize("Streamer")]
        public Task Stream3(string userId, string method, object? arg1, object? arg2, object? arg3)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3);

        [Authorize("Streamer")]
        public Task Stream4(string userId, string method, object? arg1, object? arg2, object? arg3, object? arg4)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4);

        [Authorize("Streamer")]
        public Task Stream5(string userId, string method, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4, arg5);

        [Authorize("Streamer")]
        public Task Stream6(string userId, string method, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4, arg5, arg6);

        [Authorize("Streamer")]
        public Task Stream7(string userId, string method, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        [Authorize("Streamer")]
        public Task Stream8(string userId, string method, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        [Authorize("Streamer")]
        public Task Stream9(string userId, string method, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

        [Authorize("Streamer")]
        public Task Stream10(string userId, string method, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10)
            => Clients.User(userId).SendAsync(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

    }
}
