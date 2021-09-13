using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Administration;
using Super.Paula.Auditing;
using Super.Paula.Guidlines;
using Super.Paula.Inventory;

namespace Super.Paula
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaClientTransport(this IServiceCollection services)
        {
            services.AddHttpClient<IAccountHandler, AccountHandler>();
            services.AddScoped<AccountHandlerCache>();

            services.AddHttpClient<IBusinessObjectHandler, BusinessObjectHandler>();
            services.AddHttpClient<IOrganizationHandler, OrganizationHandler>();
            services.AddHttpClient<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>();
            services.AddHttpClient<IInspectionHandler, InspectionHandler>();
            services.AddHttpClient<IInspectorHandler, InspectorHandler>();

            return services;
        }
    }
}