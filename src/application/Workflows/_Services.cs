using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Inventory;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddWorkflows(this IServiceCollection services)
        {
            services.AddScoped<BusinessObjectWorker>();

            return services;
        }
    }
}
