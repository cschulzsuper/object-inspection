using System;
using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Client.Administration;
using Super.Paula.Client.Communication;

namespace Super.Paula.Client;

public static class _Services
{
    public static IServiceCollection AddClientRadio(
        this IServiceCollection services, 
        Func<IServiceProvider, ReceiverAccessTokenProvider> accessTokenProviderFactory)
    {
        services.AddScoped<Receiver>();
        services.AddScoped<ReceiverAccessTokenProvider>(accessTokenProviderFactory);

        services.AddScoped<IInspectorCallbackHandler, InspectorCallbackHandler>();
        services.AddScoped<INotificationCallbackHandler, NotificationCallbackHandler>();

        return services;
    }
}