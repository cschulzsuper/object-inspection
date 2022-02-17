using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration
{
    public static class IdentityEndpoints
    {
        public static IEndpointRouteBuilder MapIdentity(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/identities",
                "/identities/{identity}",
                Get,
                GetAll,
                Create,
                Replace,
                Delete);

            endpoints.MapCommands(
                "/identities/{identity}",
                ("/reset", Reset));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("Maintainance")]
            (IIdentityHandler handler, string identity)
                => handler.GetAsync(identity);

        private static Delegate GetAll =>
            [Authorize("Maintainance")]
            (IIdentityHandler handler)
                => handler.GetAll();

        private static Delegate Create =>
            [Authorize("Maintainance")]
            (IIdentityHandler handler, IdentityRequest request)
                => handler.CreateAsync(request);

        private static Delegate Replace =>
            [Authorize("Maintainance")]
            (IIdentityHandler handler, string identity, IdentityRequest request)
                => handler.ReplaceAsync(identity, request);

        private static Delegate Delete =>
            [Authorize("Maintainance")]
            (IIdentityHandler handler, string identity)
                => handler.DeleteAsync(identity);

        private static Delegate Reset =>
            [Authorize("Maintainance")]
            (IIdentityHandler handler, string identity)
                => handler.ResetAsync(identity);
    }
}