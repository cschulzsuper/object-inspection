using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Guidlines;
using Super.Paula.Application.Inventory;
using Super.Paula.Client.Administration;
using Super.Paula.Client.Auditing;
using Super.Paula.Client.Guidlines;
using Super.Paula.Client.Inventory;
using Super.Paula.Client.Localization;

namespace Super.Paula.Client
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaClient(this IServiceCollection services, bool isDevelopment)
        {
            services.AddPaulaAppState();
            services.AddPaulaAppSettings();
            services.AddPaulaAppEnvironment(isDevelopment);
            services.AddPaulaClientAuthorization();

            services.AddHttpClient<IAccountHandler, AccountHandler>();
            services.AddScoped<AccountHandlerCache>();

            services.AddHttpClient<IBusinessObjectHandler, BusinessObjectHandler>();
            services.AddHttpClient<IOrganizationHandler, OrganizationHandler>();
            services.AddHttpClient<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>();
            services.AddHttpClient<IInspectionHandler, InspectionHandler>();
            services.AddHttpClient<IInspectorHandler, InspectorHandler>();

            services.AddSingleton<ITranslator,Translator>();

            return services;
        }
    }
}