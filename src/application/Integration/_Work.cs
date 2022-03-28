using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{

    [SuppressMessage("Style", "IDE1006")]
    public static class _Work
    {
        public static IWorkerRegistry ConfigureIntegration(this IWorkerRegistry workerRegistry)
        {
            workerRegistry.Register<EventStorageWorker>(c =>
            {
                c.Name = "event-storage-worker";
                c.IterationDelay = 1_000;
            });

            workerRegistry.Register<ContinuationStorageWorker>(c =>
            {
                c.Name = "continuation-storage-worker";
                c.IterationDelay = 1_000;
            });

            return workerRegistry;
        }
    }
}
