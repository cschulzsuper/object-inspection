using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddServerIntegration(this IServiceCollection services)
        {
            services.AddServerIntegrationAdministration();
            services.AddServerIntegrationInventory();
            services.AddServerIntegrationOrchestration();

            return services;
        }

        private static IServiceCollection AddServerIntegrationOrchestration(this IServiceCollection services)
        {
            services
                .AddSingleton<IEventStorage, InMemoryEventStorage>()
                .AddSingleton<IEventBus, LocalEventBus>();

            services
                .AddSingleton<IWorkerHost, LocalWorkerHost>()
                .AddSingleton<IWorkerRegistry, InMemoryWorkerRegistry>();

            services
                .AddSingleton<IContinuationStorage, InMemoryContinuationStorage>()
                .AddSingleton<IContinuator, Continuator>();

            return services;
        }

        private static IServiceCollection AddServerIntegrationAdministration(this IServiceCollection services)
        {
            services.AddScoped<ITokenAuthorizationFilter, TokenAuthorizationFilter>();

            return services;
        }

        private static IServiceCollection AddServerIntegrationInventory(this IServiceCollection services)
        {
            services.AddScoped<IBusinessObjectInspectionAuditScheduleFilter, BusinessObjectInspectionAuditScheduleFilter>();


            return services;
        }

    }
}
