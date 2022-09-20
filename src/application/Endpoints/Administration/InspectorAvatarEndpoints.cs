using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Security.Claims;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public static class InspectorAvatarEndpoints
{
    public static IEndpointRouteBuilder MapInspectorAvatar(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestResource(
            "Inspector Avatar",
            "/inspectors/{inspector}/avatar",
            Get, null, null);

        endpoints.MapRestResource(
            "Current Inspector",
            "/inspectors/me/avatar",
            GetMe,
            PutMe,
            DeleteMe);

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("OnlyInspectorOrObserver")]
        async (IInspectorAvatarRequestHandler requestHandler, string inspector)
                    => Results.File(await requestHandler.ReadAsync(inspector));

    private static Delegate GetMe =>
        [Authorize("OnlyInspectorOrObserver")]
        async (IInspectorAvatarRequestHandler requestHandler, ClaimsPrincipal user)
                    => Results.File(await requestHandler.ReadAsync(user.Claims.GetInspector()));

    private static Delegate PutMe =>
        [Authorize("OnlyInspector")]
        (IInspectorAvatarRequestHandler requestHandler, ClaimsPrincipal user, Stream body)
                    => requestHandler.WriteAsync(body, user.Claims.GetInspector());

    private static Delegate DeleteMe =>
        [Authorize("OnlyInspector")]
        (IInspectorAvatarRequestHandler requestHandler, ClaimsPrincipal user)
                    => requestHandler.DeleteAsync(user.Claims.GetInspector());
}