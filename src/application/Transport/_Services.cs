using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;

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

                .AddScoped<NotificationHandler>(InspectionHandlerFactory)
                .AddScoped<INotificationHandler>(x => x.GetRequiredService<NotificationHandler>())
                .AddScoped<INotificationEventHandler>(x => x.GetRequiredService<NotificationHandler>())

                .AddScoped<IInspectorHandler, InspectorHandler>()
                .AddScoped<IInspectionHandler, InspectionHandler>()
                .AddScoped<IOrganizationHandler, OrganizationHandler>();

            services
                .AddTransient(provider => new Lazy<IInspectionHandler>(provider.GetRequiredService<IInspectionHandler>))
                .AddTransient(provider => new Lazy<IOrganizationHandler>(provider.GetRequiredService<IOrganizationHandler>));

            return services;
        }

        private static readonly Func<IServiceProvider, NotificationHandler> InspectionHandlerFactory = 
            services =>
            {
                var notificationManager = services.GetRequiredService<INotificationManager>();
                var notificationHandler = new NotificationHandler(notificationManager);

                var notificationMessenger = services.GetRequiredService<INotificationMessenger>();

                notificationHandler.OnCreatedAsync(notificationMessenger.OnCreatedAsync);
                notificationHandler.OnDeletedAsync(notificationMessenger.OnDeletedAsync);

                return notificationHandler;
            };
    }
}