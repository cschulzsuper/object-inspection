using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using System;
using System.IO;
using System.Security.Claims;
using Super.Paula.Authorization;

namespace Super.Paula.Application.Storage
{
    public static class FileBlobEndpoints
    {
        public static IEndpointRouteBuilder MapInspectorAvatar(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapResource(
                "/inspectors/{inspector}/avatar",
                Get, null, null);

            endpoints.MapResource(
                "/inspectors/me/avatar",
                GetCurrent, 
                PutCurrent, 
                DeleteCurrent);

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("AuditingLimited")]
            async (IFileBlobHandler handler, string inspector)
                    => Results.File(await handler.ReadAsync("inspector-avatars", inspector));

        private static Delegate GetCurrent =>
            [Authorize("AuditingLimited")]
            async (IFileBlobHandler handler, ClaimsPrincipal user)
                    => Results.File(await handler.ReadAsync("inspector-avatars", user.GetInspector()));

        private static Delegate PutCurrent =>
            [Authorize("AuditingFull")]
            (IFileBlobHandler handler, ClaimsPrincipal user, [FromHeader(Name = "If-Match")] string? btag, Stream body)
                    => handler.WriteAsync(body, "inspector-avatars", user.GetInspector(), btag);

        private static Delegate DeleteCurrent =>
            [Authorize("AuditingFull")]
            (IFileBlobHandler handler, ClaimsPrincipal user, [FromHeader(Name = "If-Match")] string btag)
                    => handler.DeleteAsync("inspector-avatars", user.GetInspector(), btag);
    }
}
