using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public interface IWorkerRegistry
{
    void Register<TWorker>(Action<WorkerOptions> configure);

    bool HasWorkers();

    bool HasUnstartedWorkers();

    IAsyncEnumerable<WorkerRegistration> GetUnstartedWorkerAsync();

    ValueTask<bool> SetWorkerAsStartingAsync(string workerName);

    ValueTask SetWorkerAsRunningAsync(string workerName);

    ValueTask SetWorkerAsFinishedAsync(string workerName);

    ValueTask SetWorkerAsFailedAsync(string workerName, Exception? exception);

}