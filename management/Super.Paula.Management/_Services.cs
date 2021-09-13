using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Administration;
using Super.Paula.Auditing;
using Super.Paula.Guidlines;
using Super.Paula.Inventory;

namespace Super.Paula
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