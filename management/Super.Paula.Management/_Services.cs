using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Management.Administration;
using Super.Paula.Management.Auditing;
using Super.Paula.Management.Guidlines;
using Super.Paula.Management.Inventory;

namespace Super.Paula.Management
{
    public static class _Services
    {
        public static IServiceCollection AddPaulaManagement(this IServiceCollection services)
            => services
                .AddScoped<IBusinessObjectInspectionAuditManager, BusinessObjectInspectionAuditManager>()
                .AddScoped<IBusinessObjectManager, BusinessObjectManager>()
                .AddScoped<IInspectionManager, InspectionManager>()
                .AddScoped<IInspectorManager, InspectorManager>()
                .AddScoped<IOrganizationManager, OrganizationManager>();
    }
}