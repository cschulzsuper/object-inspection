using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
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
using Super.Paula.Environment;

namespace Super.Paula.Client
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaClient(this IServiceCollection services, 
            bool isDevelopment,
            bool isWebAssembly)
        {
            services.AddPaulaAppAuthentication();
            services.AddPaulaAppSettings();
            services.AddPaulaAppEnvironment(isDevelopment);
            services.AddPaulaClientAuthorization();

            services.AddScoped<AuthenticationMessageHandler>();

            if (!isWebAssembly)
            {
                services.AddHttpClient();
                services.AddHttpClientHandler<AccountHandlerBase>();
                services.AddHttpClientHandler<NotificationHandlerBase>();

                services.AddHttpClientHandler<IBusinessObjectHandler, BusinessObjectHandler>();
                services.AddHttpClientHandler<IOrganizationHandler, OrganizationHandler>();
                services.AddHttpClientHandler<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>();
                services.AddHttpClientHandler<IInspectionHandler, InspectionHandler>();
                services.AddHttpClientHandler<IInspectorHandler, InspectorHandler>();
            }
            else
            {
                services
                    .AddHttpClient<AccountHandlerBase>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<NotificationHandlerBase>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

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
            }

            services.AddScoped<IAccountHandler>(provider =>
                new AccountHandler(
                    provider.GetRequiredService<AccountHandlerBase>(),
                    provider.GetRequiredService<AppAuthentication>(),
                    new Lazy<AuthenticationStateManager>(() => provider.GetRequiredService<AuthenticationStateManager>())));

            services.AddScoped<INotificationHandler>(provider => 
                new NotificationHandler(
                    provider.GetRequiredService<NotificationHandlerBase>(),
                    provider.GetRequiredService<AuthenticationStateManager>()));

            services.AddSingleton<ITranslator,Translator>();

            return services;
        }

        public static IServiceCollection AddHttpClientHandler<THandler>(this IServiceCollection services)
            where THandler : class
        {
            return services.AddTransient(sp =>
            {
                var messageHandlerFactory = sp.GetRequiredService<IHttpMessageHandlerFactory>();
                var clientFactory = sp.GetRequiredService<ITypedHttpClientFactory<THandler>>();

                var factoryHandler = messageHandlerFactory.CreateHandler();
                var fullHandler = new AuthenticationMessageHandler(sp.GetRequiredService<AppAuthentication>());
                fullHandler.InnerHandler = factoryHandler;

                var httpClient = new HttpClient(fullHandler, disposeHandler: true);
                return clientFactory.CreateClient(httpClient);
            });
        }

        public static IServiceCollection AddHttpClientHandler<TService, THandler>(this IServiceCollection services)
            where TService : class
            where THandler : class, TService
        {
            return services.AddTransient<TService, THandler>(sp =>
            {
                var messageHandlerFactory = sp.GetRequiredService<IHttpMessageHandlerFactory>();
                var clientFactory = sp.GetRequiredService<ITypedHttpClientFactory<THandler>>();

                var factoryHandler = messageHandlerFactory.CreateHandler();
                var fullHandler = new AuthenticationMessageHandler(sp.GetRequiredService<AppAuthentication>());
                fullHandler.InnerHandler = factoryHandler;

                var httpClient = new HttpClient(fullHandler, disposeHandler: true);
                return clientFactory.CreateClient(httpClient);
            });
        }
    }
}