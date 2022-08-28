using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Operation;
using Super.Paula.Application.SignalR;
using System.Diagnostics.CodeAnalysis;
using Super.Paula.Application.Authentication;

namespace Super.Paula.Server;

[SuppressMessage("Style", "IDE1006")]
public static class _Endpoints
{
    public static IEndpointRouteBuilder MapServer(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapAuthentication()
            .MapAuthorization()
            .MapBusinessObject()
            .MapBusinessObjectInspector()
            .MapBusinessObjectInspection()
            .MapBusinessObjectInspectionAuditRecord()
            .MapExtension()
            .MapExtensionAggrgateType()
            .MapExtensionFieldType()
            .MapIdentity()
            .MapInspection()
            .MapInspector()
            .MapInspectorAvatar()
            .MapNotification()
            .MapOrganization();

        endpoints.MapHub<RadioHub>("/radio");

        return endpoints;
    }
}