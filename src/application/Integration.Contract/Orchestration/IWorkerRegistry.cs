using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IWorkerRegistry
    {
        void Register<TWorker>(Action<WorkerOptions> configure);

        bool Empty();

        IAsyncEnumerable<WorkerRegistration> GetUnstartedWorkerAsync();

        ValueTask SetWorkerAsStartedAsync(string workerName);

        ValueTask SetWorkerAsFinishedAsync(string workerName);

        ValueTask SetWorkerAsFailedAsync(string workerName, Exception? exception);
        
    }
}
