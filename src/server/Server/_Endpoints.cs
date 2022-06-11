using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auth;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Storage;
using Super.Paula.Application.Streaming;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Endpoints
    {
        public static IEndpointRouteBuilder MapServer(this IEndpointRouteBuilder endpoints)
            => endpoints
                .MapAuthentication()
                .MapAuthorization()
                .MapBusinessObject()
                .MapBusinessObjectInspection()
                .MapBusinessObjectInspectionAuditRecord()
                .MapIdentity()
                .MapInspection()
                .MapInspector()
                .MapInspectorAvatar()
                .MapNotification()
                .MapOrganization()
                .MapStreaming();
    }
}