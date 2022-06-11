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

namespace Super.Paula.Application.Storage
{
    public static class FileBlobEndpoints
    {
        public static IEndpointRouteBuilder MapBusinessObject(this IEndpointRouteBuilder endpoints)
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
            (IFileBlobHandler handler, string inspector, [FromHeader(Name = "If-Match")] string? btag, IFormFile formFile )
                    => handler.WriteAsync(formFile.OpenReadStream(), "inspector-avatars", inspector, btag);

        private static Delegate Get =>
            [Authorize("AuditingLimited")]
            (IFileBlobHandler handler, string inspector)
                    => handler.ReadAsync("inspector-avatars", inspector);

        private static Delegate Delete =>
            [Authorize("AuditingLimited")]
            (IFileBlobHandler handler, string inspector, [FromHeader(Name = "If-Match")] string btag)
                    => handler.DeleteAsync("inspector-avatars", inspector, btag);
    }
}
