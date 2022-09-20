using ChristianSchulz.ObjectInspection.Application.Inventory;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Diagnostics.CodeAnalysis;

namespace ChristianSchulz.ObjectInspection.Application;

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