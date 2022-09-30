using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using ChristianSchulz.ObjectInspection.Shared.Security;

namespace ChristianSchulz.ObjectInspection.Shared;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceProvider ConfigureUser(this IServiceProvider services, ClaimsPrincipal user)
    {
        var context = services.GetRequiredService<ClaimsPrincipalContext>();

        context.User = user;

        return services;
    }

    public static IServiceCollection AddUser(this IServiceCollection services)
        => services
            .AddScoped<ClaimsPrincipalContext>()
            .AddScoped(provider => provider.GetRequiredService<ClaimsPrincipalContext>().User);
}