using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Endpoints
    {
        public static IEndpointRouteBuilder MapPaulaServer(this IEndpointRouteBuilder endpoints)
            => endpoints
                .MapAccount()
                .MapBusinessObject()
                .MapBusinessObjectInspectionAudit()
                .MapIdentity()
                .MapInspection()
                .MapInspector()
                .MapNotification()
                .MapOrganization()
                .MapStreaming();
    }
}