using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Security.Claims;
using Super.Paula.Authorization;

namespace Super.Paula.Application.Administration
{
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
            [Authorize("AuditingLimited")]
            async (IInspectorAvatarHandler handler, string inspector)
                        => Results.File(await handler.ReadAsync(inspector));

        private static Delegate GetMe =>
            [Authorize("AuditingLimited")]
            async (IInspectorAvatarHandler handler, ClaimsPrincipal user)
                        => Results.File(await handler.ReadAsync(user.GetInspector()));

        private static Delegate PutMe =>
            [Authorize("AuditingFull")]
            (IInspectorAvatarHandler handler, ClaimsPrincipal user, Stream body)
                        => handler.WriteAsync(body, user.GetInspector());

        private static Delegate DeleteMe =>
            [Authorize("AuditingFull")]
            (IInspectorAvatarHandler handler, ClaimsPrincipal user)
                        => handler.DeleteAsync(user.GetInspector());
    }
}
