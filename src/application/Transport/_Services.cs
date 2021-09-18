using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidlines;
using Super.Paula.Application.Inventory;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerTransport(this IServiceCollection services)
        {
            services
                .AddScoped<IAccountHandler, AccountHandler>()
                .AddScoped<IBusinessObjectHandler, BusinessObjectHandler>()
                .AddScoped<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>()
                .AddScoped<IInspectorHandler, InspectorHandler>()
                .AddScoped<IInspectionHandler, InspectionHandler>()
                .AddScoped<INotificationHandler, NotificationHandler>(InspectionHandlerFactory)
                .AddScoped<IOrganizationHandler, OrganizationHandler>();

            services
                .AddTransient(provider => new Lazy<IBusinessObjectHandler>(() => provider.GetRequiredService<IBusinessObjectHandler>()))
                .AddTransient(provider => new Lazy<IBusinessObjectInspectionAuditHandler>(() => provider.GetRequiredService<IBusinessObjectInspectionAuditHandler>()))
                .AddTransient(provider => new Lazy<IInspectionHandler>(() => provider.GetRequiredService<IInspectionHandler>()))
                .AddTransient(provider => new Lazy<INotificationHandler>(() => provider.GetRequiredService<INotificationHandler>()))
                .AddTransient(provider => new Lazy<IOrganizationHandler>(() => provider.GetRequiredService<IOrganizationHandler>()));

            return services;
        }

        private static readonly Func<IServiceProvider, NotificationHandler> InspectionHandlerFactory = 
            (IServiceProvider services) =>
            {
                var notificationManager = services.GetRequiredService<INotificationManager>();
                var notificationHandler = new NotificationHandler(notificationManager);

                var notificationMessenger = services.GetRequiredService<INotificationMessenger>();

                notificationHandler.OnCreatedAsync(notificationMessenger.OnCreatedAsync);

                return notificationHandler;
            };
    }
}