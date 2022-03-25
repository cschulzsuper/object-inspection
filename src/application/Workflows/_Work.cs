using Super.Paula.Application.Inventory;
using Super.Paula.Application.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Application
{
    [SuppressMessage("Style", "IDE1006")]
    public static class _Work
    {
        public static IWorkerRegistry ConfigureWorkflows(this IWorkerRegistry workerRegistry)
        {
            workerRegistry.Register<BusinessObjectWorker>(c =>
            {
                c.Name = "business-object-worker";
                c.IterationDelay = 3600_000;
            });

            return workerRegistry;
        }
    }
}
