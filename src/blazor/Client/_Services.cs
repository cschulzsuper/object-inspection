using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
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
using Super.Paula.Client.Local;
using Super.Paula.Client.Streaming;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

using paulaAdministration = Super.Paula.Application.Administration;
using msftAuthorization = Microsoft.AspNetCore.Authorization;
using Super.Paula.Client.Auth;
using Super.Paula.Application.Auth;
using Super.Paula.Application.Storage;
using Super.Paula.Client.Storage;
using Super.Paula.Application.Localization;

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
            services.AddClientLocalization();

            services.AddScoped<ILocalStorage, TStorage>();

            return services;
        }

        private static IServiceCollection AddClientAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddSingleton<IAuthorizationPolicyProvider, PaulaAuthorizationPolicyProvider>();
            services.AddScoped<msftAuthorization::IAuthorizationHandler, AnyAuthorizationClaimHandler>();
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
            services.AddClientTransportAuth(isWebAssembly);
            services.AddClientTransportCommunication(isWebAssembly);
            services.AddClientTransportGuidelines(isWebAssembly);
            services.AddClientTransportInventory(isWebAssembly);
            services.AddClientTransportStorage(isWebAssembly);

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
                    .AddHttpClient<IOrganizationHandler, OrganizationHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<AuthorizationHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<AuthorizationHandler>();
                services.AddHttpClientHandler<InspectorHandler>();
                services.AddHttpClientHandler<IOrganizationHandler, OrganizationHandler>();
            }

            services.AddScoped<paulaAdministration::IAuthorizationHandler>(provider =>
                new ExtendedAuthorizationHandler(
                    provider.GetRequiredService<AuthorizationHandler>(),
                    provider.GetRequiredService<ILocalStorage>()));

            services.AddScoped<IInspectorHandler>(provider =>
                new ExtendedInspectorHandler(
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

        private static IServiceCollection AddClientTransportAuth(this IServiceCollection services, bool isWebAssembly)
        {
            if (isWebAssembly)
            {
                services
                    .AddHttpClient<IIdentityHandler, IdentityHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();

                services
                    .AddHttpClient<AuthenticationHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<AuthenticationHandler>();
                services.AddHttpClientHandler<IIdentityHandler, IdentityHandler>();
            }

            services.AddScoped<IAuthenticationHandler>(provider =>
                new ExtendedAuthenticationHandler(
                    provider.GetRequiredService<ILogger<ExtendedAuthenticationHandler>>(),
                    provider.GetRequiredService<AuthenticationHandler>(),
                    provider.GetRequiredService<ILocalStorage>()));

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

        private static IServiceCollection AddClientTransportStorage(this IServiceCollection services, bool isWebAssembly)
        {
            if (isWebAssembly)
            {
                services
                    .AddHttpClient<FileBlobHandler>()
                    .AddHttpMessageHandler<AuthenticationMessageHandler>();
            }
            else
            {
                services.AddHttpClientHandler<FileBlobHandler>();
            }

            services.AddScoped<IFileBlobHandler>(provider =>
                new ExtendedFileBlobHandler(
                    provider.GetRequiredService<ILogger<ExtendedFileBlobHandler>>(),
                    provider.GetRequiredService<FileBlobHandler>()));

            return services;
        }

        private static IServiceCollection AddClientLocalization(this IServiceCollection services)
        {
            services.AddSingleton<ITranslationHandler>(_ => TranslationHandlerFactory.Create());
            services.AddSingleton(typeof(ITranslator<>), typeof(Translator<>));
            services.AddSingleton<TranslationCategoryProvider>();

            return services;
        }
    }
}