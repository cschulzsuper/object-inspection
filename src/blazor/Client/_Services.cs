using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Application.Guidelines;
using Super.Paula.Application.Inventory;
using Super.Paula.Authorization;
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
        public static IServiceCollection AddClient<TStorage>(this IServiceCollection services,
            bool isDevelopment, bool isWebAssembly)

            where TStorage : class, ILocalStorage
        {
            services.AddAppSettings();
            services.AddAppEnvironment(isDevelopment);
            services.AddBuildInfo();

            services.AddClientAuthorization();
            services.AddClientTransport(isWebAssembly);

            services.AddScoped<ILocalStorage, TStorage>();
            services.AddSingleton<ITranslator, Translator>();

            return services;
        }

        private static IServiceCollection AddClientAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, AnyAuthorizationClaimHandler>();
            services.AddScoped<AuthenticationStateProvider, AuthenticationStateManager>();

            return services;
        }

        private static IServiceCollection AddHttpClientHandler<THandler>(this IServiceCollection services)
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

        private static IServiceCollection AddHttpClientHandler<TService, THandler>(this IServiceCollection services)
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

        private static IServiceCollection AddClientTransport(this IServiceCollection services, bool isWebAssembly)
        {
            if (!isWebAssembly)
            {
                services.AddHttpClient();
            }

            services.AddScoped<AuthenticationMessageHandler>();
            services.AddScoped<IStreamConnection, StreamConnection>();

            services.AddClientTransportAdministration(isWebAssembly);
            services.AddClientTransportAuditing(isWebAssembly);
            services.AddClientTransportCommunication(isWebAssembly);
            services.AddClientTransportGuidelines(isWebAssembly);
            services.AddClientTransportInventory(isWebAssembly);

            return services;
        }

        private static IServiceCollection AddClientTransportAdministration(this IServiceCollection services, bool isWebAssembly)
        {
            if (isWebAssembly)
            {
                services
                    .AddHttpClient<InspectorHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<IIdentityHandler, IdentityHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<IOrganizationHandler, OrganizationHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<AccountHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<AuthenticationHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<AccountHandler>();
                services.AddHttpClientHandler<AuthenticationHandler>();
                services.AddHttpClientHandler<InspectorHandler>();
                services.AddHttpClientHandler<IOrganizationHandler, OrganizationHandler>();
                services.AddHttpClientHandler<IIdentityHandler, IdentityHandler>();
            }

            services.AddScoped<IAuthenticationHandler>(provider =>
                new StoredTokenAuthenticationHandler(
                    provider.GetRequiredService<AuthenticationHandler>(),
                    provider.GetRequiredService<ILocalStorage>()));

            services.AddScoped<IAccountHandler>(provider =>
                new StoredTokenAccountHandler(
                    provider.GetRequiredService<AccountHandler>(),
                    provider.GetRequiredService<ILocalStorage>()));

            services.AddScoped<IInspectorHandler>(provider =>
                new CachedInspectorHandler(
                    provider.GetRequiredService<InspectorHandler>(),
                    provider.GetRequiredService<AuthenticationStateProvider>()));

            return services;
        }

        private static IServiceCollection AddClientTransportAuditing(this IServiceCollection services, bool isWebAssembly)
        {
            if (isWebAssembly)
            {
                services
                    .AddHttpClient<IBusinessObjectInspectionAuditRecordHandler, BusinessObjectInspectionAuditRecordHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<IBusinessObjectInspectionHandler, BusinessObjectInspectionHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<IBusinessObjectInspectionAuditRecordHandler, BusinessObjectInspectionAuditRecordHandler>();
                services.AddHttpClientHandler<IBusinessObjectInspectionHandler, BusinessObjectInspectionHandler>();
            }

            return services;
        }

        private static IServiceCollection AddClientTransportCommunication(this IServiceCollection services, bool isWebAssembly)
        {
            if (isWebAssembly)
            {
                services
                    .AddHttpClient<NotificationHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<NotificationHandler>();
            }

            services.AddScoped<INotificationHandler>(provider =>
                new CachedNotificationHandler(
                    provider.GetRequiredService<NotificationHandler>(),
                    provider.GetRequiredService<AuthenticationStateProvider>()));

            return services;
        }

        private static IServiceCollection AddClientTransportGuidelines(this IServiceCollection services, bool isWebAssembly)
        {
            if (isWebAssembly)
            {
                services
                    .AddHttpClient<IInspectionHandler, InspectionHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<IInspectionHandler, InspectionHandler>();
            }

            return services;
        }

        private static IServiceCollection AddClientTransportInventory(this IServiceCollection services, bool isWebAssembly)
        {
            if (isWebAssembly)
            {
                services
                    .AddHttpClient<IBusinessObjectHandler, BusinessObjectHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<IBusinessObjectHandler, BusinessObjectHandler>();
            }

            return services;
        }
    }
}