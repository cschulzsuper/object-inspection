﻿using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Guidlines;
using Super.Paula.Application.Inventory;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerManagement(this IServiceCollection services)
            => services
                .AddScoped<IBusinessObjectInspectionAuditManager, BusinessObjectInspectionAuditManager>()
                .AddScoped<IBusinessObjectManager, BusinessObjectManager>()
                .AddScoped<IInspectionManager, InspectionManager>()
                .AddScoped<IInspectorManager, InspectorManager>()
                .AddScoped<INotificationManager, NotificationManager>()
                .AddScoped<IOrganizationManager, OrganizationManager>();
    }
}