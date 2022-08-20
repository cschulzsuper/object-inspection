using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration;

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
        [Authorize("Maintenance")]
    (IOrganizationRequestHandler requestHandler, string organization)
            => requestHandler.GetAsync(organization);

    private static Delegate GetAll =>
        [Authorize("Maintenance")]
    (IOrganizationRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate Create =>
        [Authorize("Maintenance")]
    [UseOrganizationFromRoute]
    (IOrganizationRequestHandler requestHandler, OrganizationRequest request)
                => requestHandler.CreateAsync(request);

    private static Delegate Replace =>
        [Authorize("Maintenance")]
    (IOrganizationRequestHandler requestHandler, string organization, OrganizationRequest request)
            => requestHandler.ReplaceAsync(organization, request);

    private static Delegate Delete =>
        [Authorize("Maintenance")]
    [UseOrganizationFromRoute]
    (IOrganizationRequestHandler requestHandler, string organization, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(organization, etag);

    private static Delegate Activate =>
        [Authorize("Maintenance")]
    (IOrganizationRequestHandler requestHandler, string organization, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.ActivateAsync(organization, etag);

    private static Delegate Deactivate =>
        [Authorize("Maintenance")]
    (IOrganizationRequestHandler requestHandler, string organization, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeactivateAsync(organization, etag);
}