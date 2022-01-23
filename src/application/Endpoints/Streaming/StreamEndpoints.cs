using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Streaming;

namespace Super.Paula.Application.Communication
{
    public static class StreamEndpoints
    {
        public static IEndpointRouteBuilder MapStreaming(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<StreamHub>("/stream");

            return endpoints;
        }
    }
}
