using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Communication.Requests;
using System;

namespace Super.Paula.Application.Communication
{
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
            (INotificationRequestHandler handler, string inspector, int date, int time)
                => handler.GetAsync(inspector, date, time);

        private static Delegate GetAll =>
            [Authorize("AuditingLimited")]
            (INotificationRequestHandler handler)
                => handler.GetAll();

        private static Delegate GetAllForInspector =>
            [Authorize("AuditingLimited")]
            (INotificationRequestHandler handler, string inspector)
                => handler.GetAllForInspector(inspector);

        private static Delegate Create =>
            [Authorize("Maintainance")]
            (INotificationRequestHandler handler, string inspector, NotificationRequest request)
                => handler.CreateAsync(inspector, request);

        private static Delegate Replace =>
            [Authorize("Maintainance")]
            (INotificationRequestHandler handler, string inspector, int date, int time, NotificationRequest request)
                => handler.ReplaceAsync(inspector, date, time, request);

        private static Delegate Delete =>
            [Authorize("AuditingFull")]
            (INotificationRequestHandler handler, string inspector, int date, int time, [FromHeader(Name = "If-Match")] string etag)
                => handler.DeleteAsync(inspector, date, time, etag);
    }
}
