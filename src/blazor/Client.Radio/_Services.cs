using System;
using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Client.Administration;
using ChristianSchulz.ObjectInspection.Client.Communication;

namespace ChristianSchulz.ObjectInspection.Client;

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