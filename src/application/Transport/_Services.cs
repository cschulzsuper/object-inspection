using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Streaming;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerTransport(this IServiceCollection services)
        {
            services
                .AddScoped<IEventBus, EventBus>();

            services
                .AddScoped<IPasswordHasher<Identity>, IdentityPasswordHasher>()
                .AddScoped<IIdentityHandler, IdentityHandler>();

            services
                .AddScoped<IAccountHandler, AccountHandler>()

                .AddScoped<BusinessObjectHandler>()
                .AddScoped<IBusinessObjectHandler>(x => x.GetRequiredService<BusinessObjectHandler>())
                .AddScoped<IBusinessObjectEventHandler>(x => x.GetRequiredService<BusinessObjectHandler>())

                .AddScoped<BusinessObjectInspectionAuditHandler>()
                .AddScoped<IBusinessObjectInspectionAuditHandler>(x => x.GetRequiredService<BusinessObjectInspectionAuditHandler>())
                .AddScoped<IBusinessObjectInspectionAuditEventHandler>(x => x.GetRequiredService<BusinessObjectInspectionAuditHandler>())

                .AddScoped<NotificationHandler>(NotificationHandlerFactory)
                .AddScoped<INotificationHandler>(x => x.GetRequiredService<NotificationHandler>())
                .AddScoped<INotificationEventHandler>(x => x.GetRequiredService<NotificationHandler>())

                .AddScoped<InspectorHandler>()
                .AddScoped<IInspectorHandler>(x => x.GetRequiredService<InspectorHandler>())
                .AddScoped<IInspectorEventHandler>(x => x.GetRequiredService<InspectorHandler>())

                .AddScoped<InspectionHandler>()
                .AddScoped<IInspectionHandler>(x => x.GetRequiredService<InspectionHandler>())
                .AddScoped<IInspectionProvider>(x => x.GetRequiredService<InspectionHandler>())

                .AddScoped<OrganizationHandler>()
                .AddScoped<IOrganizationHandler>(x => x.GetRequiredService<OrganizationHandler>())
                .AddScoped<IOrganizationProvider>(x => x.GetRequiredService<OrganizationHandler>());

            return services;
        }

        private static readonly Func<IServiceProvider, NotificationHandler> NotificationHandlerFactory = 
            services =>
            {
                var notificationManager = services.GetRequiredService<INotificationManager>();
                var notificationHandler = new NotificationHandler(notificationManager);

                var notificationMessenger = services.GetRequiredService<IStreamer>();

                notificationHandler.OnCreatedAsync(notificationMessenger.StreamNotificationCreationAsync);
                notificationHandler.OnDeletedAsync(notificationMessenger.StreamNotificationDeletionAsync);

                return notificationHandler;
            };
    }
}