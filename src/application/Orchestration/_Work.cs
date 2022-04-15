using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application.Orchestration
{

    [SuppressMessage("Style", "IDE1006")]
    public static class _Work
    {
        public static IWorkerRegistry ConfigureIntegration(this IWorkerRegistry workerRegistry)
        {
            workerRegistry.Register<EventProcessingWorker>(c =>
            {
                c.Name = "event-processing-storage-worker";
                c.IterationDelay = 1_000;
            });

            workerRegistry.Register<EventWorker>(c =>
            {
                c.Name = "event-storage-worker";
                c.IterationDelay = 1_000;
            });

            workerRegistry.Register<ContinuationWorker>(c =>
            {
                c.Name = "continuation-storage-worker";
                c.IterationDelay = 1_000;
            });

            return workerRegistry;
        }
    }
}
