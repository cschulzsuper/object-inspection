using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Streaming;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaRemoteStreaming(this IServiceCollection services)
            => services
                .AddScoped<IStreamer, RemoteStreamer>();
    }
}
