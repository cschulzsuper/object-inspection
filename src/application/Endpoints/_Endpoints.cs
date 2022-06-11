using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Endpoints
    {
        public static IEndpointRouteBuilder MapCollection(this IEndpointRouteBuilder endpoints,
            string collection,
            string collectionResource,
            Delegate? get,
            Delegate? getAll,
            Delegate? create,
            Delegate? replace,
            Delegate? delete)
        {
            if (get != null)
                endpoints.MapGet(collectionResource, get);

            if (getAll != null)
                endpoints.MapGet(collection, getAll);

            if (create != null)
                endpoints.MapPost(collection, create);

            if (replace != null)
                endpoints.MapPut(collectionResource, replace);

            if (delete != null)
                endpoints.MapDelete(collectionResource, delete);

            return endpoints;
        }

        public static IEndpointRouteBuilder MapResource(this IEndpointRouteBuilder endpoints,
            string collection,
            Delegate? get,
            Delegate? replace,
            Delegate? delete)
        {
            if (get != null)
                endpoints.MapGet(collection, get);

            if (replace != null)
                endpoints.MapPut(collection, replace);

            if (delete != null)
                endpoints.MapDelete(collection, delete);

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
