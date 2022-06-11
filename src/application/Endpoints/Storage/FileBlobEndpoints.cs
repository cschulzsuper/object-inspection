using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Super.Paula.Application.Storage
{
    public static class FileBlobEndpoints
    {
        public static IEndpointRouteBuilder MapInspectorAvatar(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPut(
                "/inspectors/{inspector}/avatar", 
                Put);

            endpoints.MapGet(
                "/inspectors/{inspector}/avatar", 
                Get);

            endpoints.MapDelete(
                "/inspectors/{inspector}/avatar", 
                Delete);

            return endpoints;
        }

        private static Delegate Put =>
            [Authorize("AuditingLimited")]
            (IFileBlobHandler handler, string inspector, [FromHeader(Name = "If-Match")] string? btag, Stream body)
                    => handler.WriteAsync(body, "inspector-avatars", inspector, btag);

        private static Delegate Get =>
            async (IFileBlobHandler handler, string inspector)
                    => Results.File(await handler.ReadAsync("inspector-avatars", inspector));

        private static Delegate Delete =>
            [Authorize("AuditingLimited")]
            (IFileBlobHandler handler, string inspector, [FromHeader(Name = "If-Match")] string btag)
                    => handler.DeleteAsync("inspector-avatars", inspector, btag);
    }
}
