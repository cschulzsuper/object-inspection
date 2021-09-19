using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Super.Paula.Application.Communication.Requests;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Environment;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            endpoints.MapHub<NotificationHub>(
                "/notifications/signalr");

            return endpoints;
        }

        private static Delegate Get =>
            [Authorize("Inspector")]
            (INotificationHandler handler, string inspector, int date, int time)
                => handler.GetAsync(inspector, date, time);

        private static Delegate GetAll =>
            [Authorize("Maintainer")]
            (INotificationHandler handler)
                => handler.GetAll();

        private static Delegate GetAllForInspector =>
            [Authorize("Inspector")]
            (INotificationHandler handler, string inspector)
                => handler.GetAllForInspector(inspector);

        private static Delegate Create =>
            [Authorize("Maintainer")]
            (INotificationHandler handler, string inspector, NotificationRequest request)
                => handler.CreateAsync(inspector, request);

        private static Delegate Replace =>
            [Authorize("Maintainer")]
            (INotificationHandler handler, string inspector, int date, int time, NotificationRequest request)
                => handler.ReplaceAsync(inspector, date, time, request);

        private static Delegate Delete =>
            [Authorize("Inspector")]
            (INotificationHandler handler, string inspector, int date, int time)
                => handler.DeleteAsync(inspector, date, time);
    }
}
