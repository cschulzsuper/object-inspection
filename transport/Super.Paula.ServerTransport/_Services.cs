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
        public static IServiceCollection AddPaulaServerTransport(this IServiceCollection services)
        {
            services
                .AddScoped<IAccountHandler, AccountHandler>()
                .AddScoped<IBusinessObjectHandler, BusinessObjectHandler>()
                .AddScoped<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>()
                .AddScoped<IInspectionHandler, InspectionHandler>()
                .AddScoped<IInspectorHandler, InspectorHandler>()
                .AddScoped<IOrganizationHandler, OrganizationHandler>()

                .AddTransient(provider => new Lazy<IInspectionHandler>(() => provider.GetRequiredService<IInspectionHandler>()))
                .AddTransient(provider => new Lazy<IBusinessObjectHandler>(() => provider.GetRequiredService<IBusinessObjectHandler>()))
                .AddTransient(provider => new Lazy<IOrganizationHandler>(() => provider.GetRequiredService<IOrganizationHandler>()));


            return services;
        }
    }
}