﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Super.Paula.Application.Communication.Requests;
using System;

namespace Super.Paula.Application.Communication
{
    public static class NotificationEndpoints
    {
        public static IEndpointRouteBuilder MapNotification(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapCollection(
                "/inspectors/{inspector}/notifications",
                "/inspectors/{inspector}/notifications/{date}/{time}",
                Get,
                GetAllForInspector,
                Create,
                Replace,
                Delete);

            endpoints.MapQueries(
                "/notifications",
                ("", GetAll));

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("RequiresAuditingViewability")]
            (INotificationHandler handler, string inspector, int date, int time)
                => handler.GetAsync(inspector, date, time);

        private static Delegate GetAll =>
            [Authorize("RequiresAuditingViewability")]
            (INotificationHandler handler)
                => handler.GetAll();

        private static Delegate GetAllForInspector =>
            [Authorize("RequiresAuditingViewability")]
            (INotificationHandler handler, string inspector)
                => handler.GetAllForInspector(inspector);

        private static Delegate Create =>
            [Authorize("RequiresMaintainability")]
            (INotificationHandler handler, string inspector, NotificationRequest request)
                => handler.CreateAsync(inspector, request);

        private static Delegate Replace =>
            [Authorize("RequiresMaintainability")]
            (INotificationHandler handler, string inspector, int date, int time, NotificationRequest request)
                => handler.ReplaceAsync(inspector, date, time, request);

        private static Delegate Delete =>
            [Authorize("RequiresAuditability")]
            (INotificationHandler handler, string inspector, int date, int time)
                => handler.DeleteAsync(inspector, date, time);
    }
}
