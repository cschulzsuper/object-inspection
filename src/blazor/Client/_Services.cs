using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidlines;
using Super.Paula.Application.Inventory;
using Super.Paula.Client.Administration;
using Super.Paula.Client.Auditing;
using Super.Paula.Client.Authentication;
using Super.Paula.Client.Communication;
using Super.Paula.Client.Guidlines;
using Super.Paula.Client.Inventory;
using Super.Paula.Client.Localization;
using Super.Paula.Client.Storage;

namespace Super.Paula.Client
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaClient<TSessionStorage>(this IServiceCollection services, bool isDevelopment)
            where TSessionStorage : class, ISessionStorage
        {
            services.AddPaulaAppState();
            services.AddPaulaAppSettings();
            services.AddPaulaAppEnvironment(isDevelopment);
            services.AddPaulaClientAuthorization();

            services.AddScoped<ISessionStorage, TSessionStorage>();

            services.AddScoped<AuthenticationMessageHandler>();

            services
                .AddHttpClient<IAccountHandler, AccountHandler>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>();

            services.AddScoped<AccountHandlerCache>();

            services
                .AddHttpClient<NotificationHandlerBase>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>();

            services.AddScoped<INotificationHandler>(provider => 
                new NotificationHandler(
                    provider.GetRequiredService<NotificationHandlerBase>(),
                    provider.GetRequiredService<AuthenticationStateManager>()));

            services
                .AddHttpClient<IBusinessObjectHandler, BusinessObjectHandler>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>();

            services
                .AddHttpClient<IOrganizationHandler, OrganizationHandler>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>();

            services
                .AddHttpClient<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>();

            services
                .AddHttpClient<IInspectionHandler, InspectionHandler>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>();

            services
                .AddHttpClient<IInspectorHandler, InspectorHandler>()
                .AddHttpMessageHandler<AuthenticationMessageHandler>();

            services.AddSingleton<ITranslator,Translator>();

            return services;
        }
    }
}