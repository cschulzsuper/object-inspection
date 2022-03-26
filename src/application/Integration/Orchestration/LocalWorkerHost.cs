using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public sealed class LocalWorkerHost : IWorkerHost
    {
        private readonly IWorkerRegistry _workerRegistry;
        private readonly IServiceProvider _services;
        private readonly ILogger<LocalWorkerHost> _logger;

        private readonly int processWorkerDelay = 2_000; // 2 second

        public LocalWorkerHost(
            IWorkerRegistry workerRegistry, 
            IServiceProvider services,
            ILogger<LocalWorkerHost> logger)
        {
            _workerRegistry = workerRegistry;
            _services = services;
            _logger = logger;
        }

        public async Task StartAllWorkerAsync(CancellationToken cancellationToken)
        {
            var tasks = new Dictionary<string, Task>();

            while (
                !cancellationToken.IsCancellationRequested &&
                !_workerRegistry.HasWorkers())
            {
                await Task.Delay(processWorkerDelay, cancellationToken);
            }

            while (!cancellationToken.IsCancellationRequested)
            {

                await foreach (var worker in _workerRegistry.GetUnstartedWorkerAsync())
                {
                    var successful = await _workerRegistry.SetWorkerAsStartingAsync(worker.WorkerName);

                    if (successful)
                    {
                        tasks[worker.WorkerName] = StartWorkerAsync(worker, cancellationToken)
                            .ContinueWith(async (task, state) =>
                            {
                                if (task.IsCompletedSuccessfully)
                                {
                                    await _workerRegistry.SetWorkerAsFinishedAsync(worker.WorkerName);
                                }
                                else
                                {
                                    await _workerRegistry.SetWorkerAsFailedAsync(worker.WorkerName, task.Exception);
                                }
                            }, worker);

                        await _workerRegistry.SetWorkerAsRunningAsync(worker.WorkerName);
                    }
                }

                if (tasks.Any())
                {
                    while (tasks.Values.All(x => !x.IsCompleted))
                    {
                        foreach (var workerName in tasks.Keys)
                        {
                            await _workerRegistry.SetWorkerAsRunningAsync(workerName);
                        }

                        await Task.Delay(processWorkerDelay, cancellationToken);
                    }

                    var completedTask = await Task.WhenAny(tasks.Values);
                    var completedWorkerId = tasks.Single(x => completedTask.Equals(x.Value))
                        .Key;

                    _logger.LogWarning("A worker ran to completion ({workerId}).", completedWorkerId);

                    await Task.Delay(processWorkerDelay, cancellationToken);
                }
                else
                {
                    while (!_workerRegistry.HasUnstartedWorkers())
                    {
                        await Task.Delay(processWorkerDelay, cancellationToken);
                    }
                }
            }
        }

        private Task StartWorkerAsync(WorkerRegistration workerRegistration, CancellationToken cancellationToken)
        {
            var scope = _services.CreateScope();

            var context = new WorkerContext(scope.ServiceProvider, workerRegistration.IterationDelay);

            var worker = (IWorker?)Activator.CreateInstance(workerRegistration.WorkerType);

            if (worker == null)
            {
                _logger.LogWarning("Could not create worker ({workerType})", workerRegistration.WorkerType);
                return Task.CompletedTask;
            }

            return worker
                .ExecuteAsync(context, cancellationToken)
                .ContinueWith((_, __) => scope.Dispose(), null, cancellationToken);
        }
    }
}
