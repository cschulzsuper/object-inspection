using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Streaming;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Endpoints
    {
        public static IEndpointRouteBuilder MapServer(this IEndpointRouteBuilder endpoints)
            => endpoints
                .MapAccount()
                .MapAuthentication()
                .MapBusinessObject()
                .MapBusinessObjectInspection()
                .MapBusinessObjectInspectionAuditRecord()
                .MapIdentity()
                .MapInspection()
                .MapInspector()
                .MapNotification()
                .MapOrganization()
                .MapStreaming();
    }
}