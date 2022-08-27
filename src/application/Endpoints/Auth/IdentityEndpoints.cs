using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Auth.Requests;
using System;

namespace Super.Paula.Application.Auth;

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentity(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Identities",
            "/identities",
            "/{identity}",
            Get,
            GetAll,
            Create,
            Replace,
            Delete);

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("OnlyMaintainer")]
    (IIdentityRequestHandler requestHandler, string identity)
            => requestHandler.GetAsync(identity);

    private static Delegate GetAll =>
        [Authorize("OnlyMaintainer")]
    (IIdentityRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate Create =>
        [Authorize("OnlyMaintainer")]
    (IIdentityRequestHandler requestHandler, IdentityRequest request)
            => requestHandler.CreateAsync(request);

    private static Delegate Replace =>
        [Authorize("OnlyMaintainer")]
    (IIdentityRequestHandler requestHandler, string identity, IdentityRequest request)
            => requestHandler.ReplaceAsync(identity, request);

    private static Delegate Delete =>
        [Authorize("OnlyMaintainer")]
    (IIdentityRequestHandler requestHandler, string identity, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(identity, etag);
}