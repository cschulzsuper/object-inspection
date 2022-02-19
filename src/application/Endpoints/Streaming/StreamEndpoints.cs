using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Super.Paula.Application.Streaming
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
