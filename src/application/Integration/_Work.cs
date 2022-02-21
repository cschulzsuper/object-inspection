using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{

    [SuppressMessage("Style", "IDE1006")]
    public static class _Work
    {
        public static IWorkerRegistry ConfigureIntegration(this IWorkerRegistry workerRegistry)
        {
            workerRegistry.Register<EventStorageWorker>();

            return workerRegistry;
        }
    }
}
