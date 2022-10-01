using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using ChristianSchulz.ObjectInspection.Application.Communication;
using ChristianSchulz.ObjectInspection.Application.Guidelines;
using ChristianSchulz.ObjectInspection.Application.Inventory;
using ChristianSchulz.ObjectInspection.Client.Administration;
using ChristianSchulz.ObjectInspection.Client.Auditing;
using ChristianSchulz.ObjectInspection.Client.Communication;
using ChristianSchulz.ObjectInspection.Client.Guidelines;
using ChristianSchulz.ObjectInspection.Client.Inventory;
using ChristianSchulz.ObjectInspection.Client.Localization;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using ChristianSchulz.ObjectInspection.Application.Authentication;
using ChristianSchulz.ObjectInspection.Client.Storage;
using ChristianSchulz.ObjectInspection.Application.Localization;
using ChristianSchulz.ObjectInspection.Client.Operation;
using ChristianSchulz.ObjectInspection.Shared;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.BadgeUsage;
using ChristianSchulz.ObjectInspection.Client.Authentication;
using ChristianSchulz.ObjectInspection.Client.Security;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Client;

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
        services.AddBadgeEncoding(ClaimsJsonSerializerOptions.Options);

        services.AddClientAuthorization();
        services.AddClientLocalization();
        services.AddClientRadio(provider => provider.GetRequiredService<BadgeStorage>().GetOrDefaultAsync);
        services.AddClientTransport(isWebAssembly);

        services.AddScoped<BadgeStorage>();
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
            var fullHandler = new BadgeAuthenticationMessageHandler(sp.GetRequiredService<BadgeStorage>())
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
            var fullHandler = new BadgeAuthenticationMessageHandler(sp.GetRequiredService<BadgeStorage>())
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
                provider.GetRequiredService<BadgeStorage>()));

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
                provider.GetRequiredService<BadgeStorage>()));

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
                .AddHttpClient<IDistinctionTypeRequestHandler, DistinctionTypeRequestHandler>()
                .AddHttpMessageHandler<BadgeAuthenticationMessageHandler>();

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
            services.AddHttpClientHandler<IDistinctionTypeRequestHandler, DistinctionTypeRequestHandler>();
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