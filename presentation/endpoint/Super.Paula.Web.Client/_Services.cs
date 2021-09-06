using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Environment;
using Super.Paula.Web.Client.Handling;
using Super.Paula.Web.Client.Localization;
using Super.Paula.Web.Shared.Authentication;
using Super.Paula.Web.Shared.Authorization;
using Super.Paula.Web.Shared.Handling;
using Super.Paula.Web.Shared.Localization;

namespace Super.Paula.Web.Client
{
    public static class _Services
    {
        public static IServiceCollection AddPaulaWebClient(this IServiceCollection services, bool isDevelopment)
        {
            services.AddPaulaAppState();
            services.AddPaulaAppSettings();
            services.AddPaulaAppEnvironment(isDevelopment);

            services.AddHttpClient<IAccountHandler, AccountHandler>();
            services.AddScoped<AccountHandlerCache>();

            services.AddHttpClient<IBusinessObjectHandler, BusinessObjectHandler>();
            services.AddHttpClient<IOrganizationHandler, OrganizationHandler>();
            services.AddHttpClient<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>();
            services.AddHttpClient<IInspectionHandler, InspectionHandler>();
            services.AddHttpClient<IInspectorHandler, InspectorHandler>();

            services.AddSingleton<ITranslator, Translator>();

            return services;
        }

        public static IServiceCollection AddPaulaWebClientAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PaulaAuthorizationHandler>();

            services.AddScoped<PaulaAuthenticationStateManager>();
            services.AddScoped<AuthenticationStateProvider, PaulaAuthenticationStateManager>(provider
                 => provider.GetRequiredService<PaulaAuthenticationStateManager>());

            return services;
        }
    }
}