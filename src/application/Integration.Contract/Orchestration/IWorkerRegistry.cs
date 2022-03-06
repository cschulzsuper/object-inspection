using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IWorkerRegistry
    {
        void Register<TWorker>();

        bool HasUnstartedWorker();

        IAsyncEnumerable<WorkerRegistryEntry> GetUnstartedWorkerAsync();

        ValueTask SetWorkerAsStartedAsync(Guid workerId);

        ValueTask SetWorkerAsFinishedAsync(Guid workerId);

        ValueTask SetWorkerAsFailedAsync(Guid eventId, Exception? exception);
        
    }
}
