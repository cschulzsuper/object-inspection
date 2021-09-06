using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Data;
using Super.Paula.Environment;
using Super.Paula.Management;
using Super.Paula.Web.Server.Authentication;
using Super.Paula.Web.Server.Handling;
using Super.Paula.Web.Shared.Authentication;
using Super.Paula.Web.Shared.Authorization;
using Super.Paula.Web.Shared.Handling;

namespace Super.Paula.Web.Server
{
    public static class _Services
    {
        public static IServiceCollection AddPaulaWebServer(this IServiceCollection services, bool isDevlopment)
        {
            services
                .AddPaulaData()
                .AddPaulaAppSettings()
                .AddPaulaAppEnvironment(isDevlopment)
                .AddPaulaAppState()
                .AddPaulaAppAuthentication()
                .AddPaulaManagement();

            services
                .AddScoped<IAccountHandler, AccountHandler>()
                .AddScoped<IBusinessObjectHandler, BusinessObjectHandler>()
                .AddScoped<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>()
                .AddScoped<IInspectionHandler, InspectionHandler>()
                .AddScoped<IInspectionBusinessObjectHandler, InspectionBusinessObjectHandler>()
                .AddScoped<IInspectorHandler, InspectorHandler>()
                .AddScoped<IOrganizationHandler, OrganizationHandler>()

                .AddTransient(provider => new Lazy<IInspectionHandler>(() => 
                    provider.GetRequiredService<IInspectionHandler>()))

                .AddTransient(provider => new Lazy<IBusinessObjectHandler>(() => 
                    provider.GetRequiredService<IBusinessObjectHandler>()));


            return services;
        }

        public static IServiceCollection AddPaulaWebServerAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "bearer";
                options.AddScheme<PaulaAuthenticationHandler>("bearer", null);
            });

            return services;
        }

        public static IServiceCollection AddPaulaWebServerAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PaulaAuthorizationHandler>();

            services.AddScoped<PaulaAuthenticationStateManager>();
            services.AddScoped<AuthenticationStateProvider,PaulaAuthenticationStateManager>(provider 
                => provider.GetRequiredService<PaulaAuthenticationStateManager>());

            return services;
        }
    }
}