using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Auth;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Setup;
using Super.Paula.Application.Streaming;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddServerTransport(this IServiceCollection services)
        {
            services.AddServerTransportAdministration();
            services.AddServerTransportAuditing();
            services.AddServerTransportAuth();
            services.AddServerTransportCommunication();
            services.AddServerTransportGuidelines();
            services.AddServerTransportInventory();
            services.AddServerTransportSetup();

            return services;
        }

        private static IServiceCollection AddServerTransportAdministration(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
            services.AddScoped<IAuthorizationTokenHandler, AuthorizationTokenHandler>();

            services.AddScoped<IInspectorAnnouncer>(InspectorAnnouncerFactory);
            services.AddScoped<IInspectorHandler, InspectorHandler>();
            services.AddScoped<IInspectorEventService, InspectorEventService>();
            services.AddScoped<IInspectorContinuationService, InspectorContinuationService>();

            services.AddScoped<IInspectorAvatarHandler, InspectorAvatarHandler>();

            services.AddScoped<IOrganizationHandler, OrganizationHandler>();
            services.AddScoped<IOrganizationQueries, OrganizationQueries>();
            services.AddScoped<IOrganizationEventService, OrganizationEventService>();
            services.AddScoped<IOrganizationContinuationService, OrganizationContinuationService>();

            return services;
        }

        private static IServiceCollection AddServerTransportAuditing(this IServiceCollection services)
        {
            services.AddScoped<IBusinessObjectInspectionAuditRecordHandler, BusinessObjectInspectionAuditRecordHandler>();
            services.AddScoped<IBusinessObjectInspectionContinuationService, BusinessObjectInspectionContinuationService>();
            services.AddScoped<IBusinessObjectInspectionAuditScheduler, BusinessObjectInspectionAuditScheduler>();
            services.AddScoped<IBusinessObjectInspectionHandler, BusinessObjectInspectionHandler>();
            services.AddScoped<IBusinessObjectInspectionEventService, BusinessObjectInspectionEventService>();

            return services;
        }

        private static IServiceCollection AddServerTransportAuth(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<Identity>, IdentityPasswordHasher>();
            services.AddScoped<IIdentityHandler, IdentityHandler>();
            services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();

            return services;
        }

        private static IServiceCollection AddServerTransportCommunication(this IServiceCollection services)
        {
            services.AddScoped<INotificationAnnouncer>(NotificationAnnouncerFactory);
            services.AddScoped<INotificationHandler, NotificationHandler>();

            return services;
        }

        private static IServiceCollection AddServerTransportGuidelines(this IServiceCollection services)
        {
            services.AddScoped<IInspectionHandler, InspectionHandler>();
            services.AddScoped<IInspectionEventService, InspectionEventService>();

            return services;
        }

        private static IServiceCollection AddServerTransportInventory(this IServiceCollection services)
        {
            services.AddScoped<IBusinessObjectHandler, BusinessObjectHandler>();
            services.AddScoped<IBusinessObjectQueries, BusinessObjectQueries>();
            services.AddScoped<IBusinessObjectEventService, BusinessObjectEventService>();

            return services;
        }

        private static IServiceCollection AddServerTransportSetup(this IServiceCollection services)
        {
            services.AddScoped<IExtensionHandler, ExtensionHandler>();
            services.AddScoped<IExtensionTypeHandler, ExtensionTypeHandler>();
            services.AddScoped<IExtensionFieldTypeHandler, ExtensionFieldTypeHandler>();

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