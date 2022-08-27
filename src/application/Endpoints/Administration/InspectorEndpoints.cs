using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Administration.Requests;
using System;

namespace Super.Paula.Application.Administration;

public static class InspectorEndpoints
{
    public static IEndpointRouteBuilder MapInspector(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Inspectors",
            "/inspectors",
            "/{inspector}",
            Get,
            GetAll,
            Create,
            Replace,
            Delete);

        endpoints.MapRestResouceCommands(
            "Inspectors",
            "/inspectors/{inspector}",
            ("/activate", Activate),
            ("/deactivate", Deactivate));

        endpoints.MapRestCollectionQueries(
            "Organization Inspectors",
            "/organizations",
            ("/{organization}/inspectors", GetAllForOrganization));

        endpoints.MapRestCollectionQueries(
            "Current Inspector",
            "/inspectors",
            ("/me", GetCurrent));

        endpoints.MapRestCollectionQueries(
            "Identity Inspectors",
            "/identities",
            ("/{identity}/inspectors", GetAllForIdentity));

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("OnlyChiefOrObserverOrInspectorOwner")]
    (IInspectorRequestHandler requestHandler, string inspector)
            => requestHandler.GetAsync(inspector);

    private static Delegate GetCurrent =>
        [Authorize("OnlyInspectorOrObserver")]
    (IInspectorRequestHandler requestHandler)
            => requestHandler.GetCurrentAsync();

    private static Delegate GetAll =>
        [Authorize("OnlyChiefOrObserver")]
    (IInspectorRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate GetAllForOrganization =>
        [Authorize("OnlyChiefOrObserver")]
    [UseOrganizationFromRoute]
    (IInspectorRequestHandler requestHandler, string organization)
            => requestHandler.GetAllForOrganization(organization);

    private static Delegate GetAllForIdentity =>
        [Authorize("OnlyMaintainerOrIdentityOwner")]
    (IInspectorRequestHandler requestHandler, string identity)
            => requestHandler.GetAllForIdentity(identity);

    private static Delegate Create =>
        [Authorize("OnlyChief")]
    (IInspectorRequestHandler requestHandler, InspectorRequest request)
            => requestHandler.CreateAsync(request);

    private static Delegate Replace =>
        [Authorize("OnlyChief")]
    (IInspectorRequestHandler requestHandler, string inspector, InspectorRequest request)
            => requestHandler.ReplaceAsync(inspector, request);

    private static Delegate Delete =>
        [Authorize("OnlyChief")]
    (IInspectorRequestHandler requestHandler, string inspector, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(inspector, etag);

    private static Delegate Activate =>
        [Authorize("OnlyChief")]
    (IInspectorRequestHandler requestHandler, string inspector, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.ActivateAsync(inspector, etag);

    private static Delegate Deactivate =>
       [Authorize("OnlyChief")]
    (IInspectorRequestHandler requestHandler, string inspector, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeactivateAsync(inspector, etag);
}