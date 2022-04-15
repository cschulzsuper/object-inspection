using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application.Orchestration
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Services
    {
        public static IServiceCollection AddServerIntegration(this IServiceCollection services)
        {
            services.AddServerIntegrationOrchestration();

            return services;
        }

        private static IServiceCollection AddServerIntegrationOrchestration(this IServiceCollection services)
        {
            services
                .AddScoped<IEventStorage, PersistentEventStorage>()
                .AddSingleton<IEventBus, LocalEventBus>()
                .AddScoped<IEventProcessingStorage, PersistentEventProcessingStorage>()
                .AddSingleton<IEventProcessor, EventProcessor>()
                .AddSingleton<IEventTypeRegistry, InMemoryEventTypeRegistry>();

            services
                .AddSingleton<IWorkerHost, LocalWorkerHost>()
                .AddSingleton<IWorkerRegistry, PersistentWorkerRegistry>();

            services
                .AddScoped<IContinuationStorage, PersistentContinuationStorage>()
                .AddSingleton<IContinuationRegistry, InMemoryContinuationRegistry>()
                .AddSingleton<IContinuator, Continuator>();

            return services;
        }
    }
}
