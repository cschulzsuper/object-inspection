using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Inventory;
using System.Diagnostics.CodeAnalysis;

namespace ChristianSchulz.ObjectInspection.Application;

[SuppressMessage("Style", "IDE1006")]
public static class _Services
{
    public static IServiceCollection AddWorkflows(this IServiceCollection services)
    {
        services.AddScoped<BusinessObjectWorker>();

        return services;
    }
}