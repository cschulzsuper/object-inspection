using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ChristianSchulz.ObjectInspection.Application.Administration.Requests;
using System;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public static class OrganizationEndpoints
{
    public static IEndpointRouteBuilder MapOrganization(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Organizations",
            "/organizations",
            "/{organization}",
            Get,
            GetAll,
            Create,
            Replace,
            Delete);

        endpoints.MapRestResouceCommands(
            "Organizations",
            "/organizations/{organization}",
            ("/activate", Activate),
            ("/deactivate", Deactivate));

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("OnlyMaintainer")]
    (IOrganizationRequestHandler requestHandler, string organization)
            => requestHandler.GetAsync(organization);

    private static Delegate GetAll =>
        [Authorize("OnlyMaintainer")]
    (IOrganizationRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate Create =>
        [Authorize("OnlyMaintainer")]
    [UseOrganizationFromRoute]
    (IOrganizationRequestHandler requestHandler, OrganizationRequest request)
                => requestHandler.CreateAsync(request);

    private static Delegate Replace =>
        [Authorize("OnlyMaintainer")]
    (IOrganizationRequestHandler requestHandler, string organization, OrganizationRequest request)
            => requestHandler.ReplaceAsync(organization, request);

    private static Delegate Delete =>
        [Authorize("OnlyMaintainer")]
    [UseOrganizationFromRoute]
    (IOrganizationRequestHandler requestHandler, string organization, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(organization, etag);

    private static Delegate Activate =>
        [Authorize("OnlyMaintainer")]
    (IOrganizationRequestHandler requestHandler, string organization, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.ActivateAsync(organization, etag);

    private static Delegate Deactivate =>
        [Authorize("OnlyMaintainer")]
    (IOrganizationRequestHandler requestHandler, string organization, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeactivateAsync(organization, etag);
}