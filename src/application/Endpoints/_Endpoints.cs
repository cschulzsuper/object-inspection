using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Endpoints
    {
        public const string RouteSeparator = "/";

        public static IEndpointRouteBuilder MapRestCollection(this IEndpointRouteBuilder endpoints,
            string tag,
            string collection,
            string resource,
            Delegate? get,
            Delegate? getAll,
            Delegate? create,
            Delegate? replace,
            Delegate? delete)
        {
            var collectionGroup = endpoints
                .MapGroup(collection)
                .WithTags(tag);

            if (getAll != null)
                collectionGroup.MapGet(RouteSeparator, getAll);

            if (create != null)
                collectionGroup.MapPost(RouteSeparator, create);

            var resourceGroup = collectionGroup.MapGroup(resource);

            if (get != null)
                resourceGroup.MapGet(RouteSeparator, get);

            if (replace != null)
                resourceGroup.MapPut(RouteSeparator, replace);

            if (delete != null)
                resourceGroup.MapDelete(RouteSeparator, delete);

            return endpoints;
        }

        public static IEndpointRouteBuilder MapRestResource(this IEndpointRouteBuilder endpoints,
            string tag,
            string resource,
            Delegate? get,
            Delegate? replace,
            Delegate? delete)
        {
            var resourceGroup = endpoints.MapGroup(resource);

            if (tag != null)
                resourceGroup.WithTags(tag);

            if (get != null)
                resourceGroup.MapGet(RouteSeparator, get);

            if (replace != null)
                resourceGroup.MapPut(RouteSeparator, replace);

            if (delete != null)
                resourceGroup.MapDelete(RouteSeparator, delete);

            return endpoints;
        }

        public static IEndpointRouteBuilder MapRestCollectionQueries(this IEndpointRouteBuilder endpoints,
            string tag,
            string collection,
            params (string, Delegate)[] queries)
        {
            var collectionGroup = endpoints
                .MapGroup(collection)
                .WithTags(tag);

            foreach (var query in queries)
            {
                collectionGroup.MapGet($"{query.Item1}", query.Item2);
            }

            return endpoints;
        }

        public static IEndpointRouteBuilder MapRestResouceCommands(this IEndpointRouteBuilder endpoints,
            string tag,
            string resource,
            params (string, Delegate)[] posts)
        {
            var resourceGroup = endpoints
                .MapGroup(resource)
                .WithTags(tag);

            foreach (var post in posts)
            {
                resourceGroup.MapPost($"{post.Item1}", post.Item2);
            }

            return endpoints;
        }

        [Obsolete]
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

        [Obsolete]
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

        [Obsolete]
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

        [Obsolete]
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
