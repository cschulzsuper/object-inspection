using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using ChristianSchulz.ObjectInspection.Application.Communication;
using ChristianSchulz.ObjectInspection.Application.Guidelines;
using ChristianSchulz.ObjectInspection.Application.Inventory;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Application.SignalR;
using System.Diagnostics.CodeAnalysis;
using ChristianSchulz.ObjectInspection.Application.Authentication;

namespace ChristianSchulz.ObjectInspection.Server;

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
            .MapDistinctionType()
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