using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _EndpointsExtension
    {
        public static IEndpointRouteBuilder MapCollection(this IEndpointRouteBuilder endpoints,
            string collection,
            string collectionResource,
            Delegate get,
            Delegate getAll,
            Delegate create,
            Delegate replace,
            Delegate delete)
        {
            endpoints.MapGet(collectionResource, get);
            endpoints.MapGet(collection, getAll);
            endpoints.MapPost(collection, create);
            endpoints.MapPut(collectionResource, replace);
            endpoints.MapDelete(collectionResource, delete);

            return endpoints;
        }

        public static IEndpointRouteBuilder MapQueries(this IEndpointRouteBuilder endpoints,
            string path,
            params (string, Delegate)[] queries)
        {
            foreach (var query in queries)
            {
                endpoints.MapGet($"{path}{query.Item1}", query.Item2);
            }

            return endpoints;
        }

        public static IEndpointRouteBuilder MapCommands(this IEndpointRouteBuilder endpoints,
            string path,
            params (string, Delegate)[] posts)
        {
            foreach (var post in posts)
            {
                endpoints.MapPost($"{path}{post.Item1}", post.Item2);
            }

            return endpoints;
        }
    }
}
