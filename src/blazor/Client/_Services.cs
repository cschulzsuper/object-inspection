using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Client.Administration;
using Super.Paula.Client.Auditing;
using Super.Paula.Client.Authentication;
using Super.Paula.Client.Communication;
using Super.Paula.Client.Guidelines;
using Super.Paula.Client.Inventory;
using Super.Paula.Client.Localization;
using Super.Paula.Client.Storage;
using Super.Paula.Client.Streaming;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace Super.Paula.Client
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddPaulaClient(this IServiceCollection services,
            bool isDevelopment,
            bool isWebAssembly)
        {
            services.AddPaulaAppSettings();
            services.AddPaulaAppEnvironment(isDevelopment);
            services.AddPaulaClientAuthorization();

            if (!isWebAssembly)
            {
                services.AddScoped<AuthenticationMessageHandler>();

                services.AddHttpClient();
                services.AddHttpClientHandler<AccountHandlerBase>();
                services.AddHttpClientHandler<AuthenticationHandlerBase>();
                services.AddHttpClientHandler<NotificationHandlerBase>();
                services.AddHttpClientHandler<InspectorHandlerBase>();

                services.AddHttpClientHandler<IBusinessObjectHandler, BusinessObjectHandler>();
                services.AddHttpClientHandler<IOrganizationHandler, OrganizationHandler>();
                services.AddHttpClientHandler<IBusinessObjectInspectionAuditHandler, BusinessObjectInspectionAuditHandler>();
                services.AddHttpClientHandler<IInspectionHandler, InspectionHandler>();
                services.AddHttpClientHandler<IIdentityHandler, IdentityHandler>();
            }
            else
            {
                services.AddScoped<AuthenticationMessageHandler>();
                services
                    .AddHttpClient<AccountHandlerBase>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<AuthenticationHandlerBase>()
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
                    .AddHttpClient<InspectorHandlerBase>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<IIdentityHandler, IdentityHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }

            services.AddScoped<IAccountHandler>(provider =>
                new AccountHandler(
                    provider.GetRequiredService<AccountHandlerBase>(),
                    provider.GetRequiredService<ILocalStorage>()));

            services.AddScoped<IAuthenticationHandler>(provider =>
                new AuthenticationHandler(
                    provider.GetRequiredService<AuthenticationHandlerBase>(),
                    provider.GetRequiredService<ILocalStorage>()));

            services.AddScoped<INotificationHandler>(provider =>
                new NotificationHandler(
                    provider.GetRequiredService<NotificationHandlerBase>(),
                    provider.GetRequiredService<AuthenticationStateProvider>()));

            services.AddScoped<IInspectorHandler>(provider =>
                new InspectorHandler(
                    provider.GetRequiredService<InspectorHandlerBase>(),
                    provider.GetRequiredService<AuthenticationStateProvider>()));

            services.AddSingleton<ITranslator, Translator>();
            services.AddScoped<IStreamConnection, StreamConnection>();

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
                var fullHandler = new AuthenticationMessageHandler(sp.GetRequiredService<ILocalStorage>())
                {
                    InnerHandler = factoryHandler
                };

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
                var fullHandler = new AuthenticationMessageHandler(sp.GetRequiredService<ILocalStorage>())
                {
                    InnerHandler = factoryHandler
                };

                var httpClient = new HttpClient(fullHandler, disposeHandler: true);
                return clientFactory.CreateClient(httpClient);
            });
        }
    }
}