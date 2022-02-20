using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Operation;
using Super.Paula.Application.Streaming;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerTransport(this IServiceCollection services)
        {
            services
                .AddScoped<IPasswordHasher<Identity>, IdentityPasswordHasher>()
                .AddScoped<IIdentityHandler, IdentityHandler>();

            services
                .AddScoped<IAccountHandler, AccountHandler>()
                .AddScoped<IAuthenticationHandler, AuthenticationHandler>()

                .AddScoped<IBusinessObjectHandler, BusinessObjectHandler>()
                .AddSingleton<IBusinessObjectEventHandler, BusinessObjectEventHandler>()

                .AddScoped<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>()
                .AddSingleton<IBusinessObjectInspectionAuditEventHandler, BusinessObjectInspectionAuditEventHandler>()

                .AddScoped<INotificationAnnouncer>(NotificationAnnouncerFactory)
                .AddScoped<INotificationHandler, NotificationHandler>()
                .AddSingleton<INotificationEventHandler, NotificationEventHandler>()

                .AddScoped<IInspectorAnnouncer>(InspectorAnnouncerFactory)
                .AddScoped<IInspectorHandler, InspectorHandler>()
                .AddSingleton<IInspectorEventHandler, InspectorEventHandler>()

                .AddScoped<IInspectionHandler, InspectionHandler>()
                .AddScoped<IOrganizationHandler, OrganizationHandler>()

                .AddSingleton<IApplicationEventHandler, ApplicationEventHandler>();

            return services;
        }

        private static readonly Func<IServiceProvider, INotificationAnnouncer> NotificationAnnouncerFactory =
            services =>
            {
                var notificationAnnouncer = new NotificationAnnouncer();

                var streamer = services.GetRequiredService<IStreamer>();

                notificationAnnouncer.OnCreationAsync(streamer.StreamNotificationCreationAsync);
                notificationAnnouncer.OnDeletionAsync(streamer.StreamNotificationDeletionAsync);

                return notificationAnnouncer;
            };

        private static readonly Func<IServiceProvider, IInspectorAnnouncer> InspectorAnnouncerFactory =
            services =>
            {
                var inspectorAnnouncer = new InspectorAnnouncer();

                var streamer = services.GetRequiredService<IStreamer>();

                inspectorAnnouncer.OnBusinessObjectCreationAsync(streamer.StreamInspectorBusinessObjectCreationAsync);
                inspectorAnnouncer.OnBusinessObjectDeletionAsync(streamer.StreamInspectorBusinessObjectDeletionAsync);
                inspectorAnnouncer.OnBusinessObjectUpdateAsync(streamer.StreamInspectorBusinessObjectUpdateAsync);

                return inspectorAnnouncer;
            };
    }
}