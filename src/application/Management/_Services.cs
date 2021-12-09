using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Runtime;
using System.Diagnostics.CodeAnalysis;
using Super.Paula.Application.Communication;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaServerManagement(this IServiceCollection services)
            => services
                .AddScoped<IBusinessObjectInspectionAuditManager, BusinessObjectInspectionAuditManager>()
                .AddScoped<IBusinessObjectManager, BusinessObjectManager>()
                .AddScoped<IIdentityManager, IdentityManager>()
                .AddScoped<IInspectionManager, InspectionManager>()
                .AddScoped<IInspectorManager, InspectorManager>()
                .AddScoped<INotificationManager, NotificationManager>()
                .AddScoped<IOrganizationManager, OrganizationManager>()
                .AddScoped<IConnectionManager, ConnectionManager>()
                .AddScoped<IConnectionViolationManager, ConnectionViolationManager>();
    }
}