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
using Super.Paula.Client.Administration;
using Super.Paula.Client.Auditing;
using Super.Paula.Client.Communication;
using Super.Paula.Client.Guidelines;
using Super.Paula.Client.Inventory;
using Super.Paula.Client.Localization;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Super.Paula.Application.Authentication;
using Super.Paula.Client.Storage;
using Super.Paula.Application.Localization;
using Super.Paula.Client.Operation;
using Super.Paula.Shared;
using Super.Paula.Application.Operation;
using Super.Paula.Client.Authentication;
using Super.Paula.Client.Security;
using Super.Paula.Shared.Security;

namespace Super.Paula.Client;

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
        services.AddClientLocalization();
        services.AddClientRadio();
        services.AddClientTransport(isWebAssembly);

        services.AddScoped<ILocalStorage, TStorage>();

        return services;
    }

    private static IServiceCollection AddClientAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationCore();
        services.AddSingleton<IAuthorizationPolicyProvider, BadgeAuthorizationPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, AnyAuthorizationClaimHandler>();
        services.AddScoped<AuthenticationStateProvider, BadgeAuthenticationStateProvider>();

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
            var fullHandler = new BadgeAuthenticationMessageHandler(sp.GetRequiredService<ILocalStorage>())
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
            var fullHandler = new BadgeAuthenticationMessageHandler(sp.GetRequiredService<ILocalStorage>())
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

        services.AddScoped<BadgeAuthenticationMessageHandler>();

        services.AddClientTransportAdministration(isWebAssembly);
        services.AddClientTransportAuditing(isWebAssembly);
        services.AddClientTransportAuth(isWebAssembly);
        services.AddClientTransportCommunication(isWebAssembly);
        services.AddClientTransportGuidelines(isWebAssembly);
        services.AddClientTransportInventory(isWebAssembly);
        services.AddClientTransportOperation(isWebAssembly);

        return services;
    }

    private static IServiceCollection AddClientTransportAdministration(this IServiceCollection services, bool isWebAssembly)
    {
        if (isWebAssembly)
        {
            services
                .AddHttpClient<InspectorRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<IInspectorAvatarRequestHandler, InspectorAvatarRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<IOrganizationRequestHandler, OrganizationRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<AuthorizationRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();
        }
        else
        {
            services.AddHttpClientHandler<AuthorizationRequestHandler>();
            services.AddHttpClientHandler<InspectorRequestHandler>();
            services.AddHttpClientHandler<IInspectorAvatarRequestHandler, InspectorAvatarRequestHandler>();
            services.AddHttpClientHandler<IOrganizationRequestHandler, OrganizationRequestHandler>();
        }

        services.AddScoped<IAuthorizationRequestHandler>(provider =>
            new ExtendedAuthorizationRequestHandler(
                provider.GetRequiredService<AuthorizationRequestHandler>(),
                provider.GetRequiredService<ILocalStorage>()));

        services.AddScoped<IInspectorRequestHandler>(provider =>
            new ExtendedInspectorRequestHandler(
                provider.GetRequiredService<InspectorRequestHandler>(),
                provider.GetRequiredService<IInspectorCallbackHandler>(),
                provider.GetRequiredService<AuthenticationStateProvider>()));

        return services;
    }

    private static IServiceCollection AddClientTransportAuditing(this IServiceCollection services, bool isWebAssembly)
    {
        if (isWebAssembly)
        {
            services
                .AddHttpClient<IBusinessObjectInspectionAuditRecordRequestHandler, BusinessObjectInspectionAuditRecordRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<IBusinessObjectInspectionRequestHandler, BusinessObjectInspectionRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<IBusinessObjectInspectorRequestHandler, BusinessObjectInspectorRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();
        }
        else
        {
            services.AddHttpClientHandler<IBusinessObjectInspectionAuditRecordRequestHandler, BusinessObjectInspectionAuditRecordRequestHandler>();
            services.AddHttpClientHandler<IBusinessObjectInspectionRequestHandler, BusinessObjectInspectionRequestHandler>();
            services.AddHttpClientHandler<IBusinessObjectInspectorRequestHandler, BusinessObjectInspectorRequestHandler>();
        }

        return services;
    }

    private static IServiceCollection AddClientTransportAuth(this IServiceCollection services, bool isWebAssembly)
    {
        if (isWebAssembly)
        {
            services
                .AddHttpClient<IIdentityRequestHandler, IdentityRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<AuthenticationRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();
        }
        else
        {
            services.AddHttpClientHandler<AuthenticationRequestHandler>();
            services.AddHttpClientHandler<IIdentityRequestHandler, IdentityRequestHandler>();
        }

        services.AddScoped<IAuthenticationRequestHandler>(provider =>
            new ExtendedAuthenticationRequestHandler(
                provider.GetRequiredService<ILogger<ExtendedAuthenticationRequestHandler>>(),
                provider.GetRequiredService<AuthenticationRequestHandler>(),
                provider.GetRequiredService<ILocalStorage>()));

        return services;
    }

    private static IServiceCollection AddClientTransportCommunication(this IServiceCollection services, bool isWebAssembly)
    {
        if (isWebAssembly)
        {
            services
                .AddHttpClient<NotificationRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();
        }
        else
        {
            services.AddHttpClientHandler<NotificationRequestHandler>();
        }

        services.AddScoped<INotificationRequestHandler>(provider =>
            new CachedNotificationRequestHandler(
                provider.GetRequiredService<NotificationRequestHandler>(),
                provider.GetRequiredService<INotificationCallbackHandler>(),
                provider.GetRequiredService<AuthenticationStateProvider>()));

        return services;
    }

    private static IServiceCollection AddClientTransportGuidelines(this IServiceCollection services, bool isWebAssembly)
    {
        if (isWebAssembly)
        {
            services
                .AddHttpClient<IInspectionRequestHandler, InspectionRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();
        }
        else
        {
            services.AddHttpClientHandler<IInspectionRequestHandler, InspectionRequestHandler>();
        }

        return services;
    }

    private static IServiceCollection AddClientTransportInventory(this IServiceCollection services, bool isWebAssembly)
    {
        if (isWebAssembly)
        {
            services
                .AddHttpClient<IBusinessObjectRequestHandler, BusinessObjectRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();
        }
        else
        {
            services.AddHttpClientHandler<IBusinessObjectRequestHandler, BusinessObjectRequestHandler>();
        }

        return services;
    }
    private static IServiceCollection AddClientTransportOperation(this IServiceCollection services, bool isWebAssembly)
    {
        if (isWebAssembly)
        {
            services
                .AddHttpClient<IExtensionRequestHandler, ExtensionRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<IExtensionAggregateTypeRequestHandler, ExtensionAggregateTypeRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

            services
                .AddHttpClient<IExtensionFieldTypeRequestHandler, ExtensionFieldTypeRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();
        }
        else
        {
            services.AddHttpClientHandler<IExtensionRequestHandler, ExtensionRequestHandler>();
            services.AddHttpClientHandler<IExtensionAggregateTypeRequestHandler, ExtensionAggregateTypeRequestHandler>();
            services.AddHttpClientHandler<IExtensionFieldTypeRequestHandler, ExtensionFieldTypeRequestHandler>();
        }

        return services;
    }

    private static IServiceCollection AddClientLocalization(this IServiceCollection services)
    {
        services.AddSingleton<ITranslationRequestHandler>(_ => TranslationRequestHandlerFactory.Create());
        services.AddSingleton(typeof(ITranslator<>), typeof(Translator<>));
        services.AddSingleton<TranslationCategoryProvider>();

        return services;
    }
}