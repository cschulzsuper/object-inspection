using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Communication.Requests;
using System;

namespace Super.Paula.Application.Communication;

public static class NotificationEndpoints
{
    public static IEndpointRouteBuilder MapNotification(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRestCollection(
            "Inspector Notifications",
            "/inspectors/{inspector}/notifications",
            "/{date}/{time}",
            Get,
            GetAllForInspector,
            Create,
            Replace,
            Delete);

        endpoints.MapRestCollectionQueries(
            "Notifications",
            "/notifications",
            ("", GetAll));

        return endpoints;
    }

    private static Delegate Get =>
        [Authorize("AuditingLimited")]
    (INotificationRequestHandler requestHandler, string inspector, int date, int time)
            => requestHandler.GetAsync(inspector, date, time);

    private static Delegate GetAll =>
        [Authorize("AuditingLimited")]
    (INotificationRequestHandler requestHandler)
            => requestHandler.GetAll();

    private static Delegate GetAllForInspector =>
        [Authorize("AuditingLimited")]
    (INotificationRequestHandler requestHandler, string inspector)
            => requestHandler.GetAllForInspector(inspector);

    private static Delegate Create =>
        [Authorize("Maintenance")]
    (INotificationRequestHandler requestHandler, string inspector, NotificationRequest request)
            => requestHandler.CreateAsync(inspector, request);

    private static Delegate Replace =>
        [Authorize("Maintenance")]
    (INotificationRequestHandler requestHandler, string inspector, int date, int time, NotificationRequest request)
            => requestHandler.ReplaceAsync(inspector, date, time, request);

    private static Delegate Delete =>
        [Authorize("AuditingFull")]
    (INotificationRequestHandler requestHandler, string inspector, int date, int time, [FromHeader(Name = "If-Match")] string etag)
            => requestHandler.DeleteAsync(inspector, date, time, etag);
}