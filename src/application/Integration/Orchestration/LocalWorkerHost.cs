using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public sealed class LocalWorkerHost : IWorkerHost
    {
        private readonly IWorkerRegistry _workerRegistry;
        private readonly IServiceProvider _services;
        private readonly ILogger<LocalWorkerHost> _logger;

        private readonly int restartWorkerDelay = 60_000; // 1 minutes
        private readonly int startUpWorkerDelay = 1_000; // 1 second

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
            var tasks = new Dictionary<Guid, Task?>();

            while (
                !cancellationToken.IsCancellationRequested &&
                !_workerRegistry.HasUnstartedWorker())
            {
                await Task.Delay(startUpWorkerDelay, cancellationToken);
            }

            while (!cancellationToken.IsCancellationRequested)
            {

                await foreach (var worker in _workerRegistry.GetUnstartedWorkerAsync())
                {
                    await _workerRegistry.SetWorkerAsStartedAsync(worker.Id);

                    tasks[worker.Id] = StartWorkerAsync(worker, cancellationToken)?
                        .ContinueWith(async (task, state) =>
                        {
                            if (task.IsCompletedSuccessfully)
                            {
                                await _workerRegistry.SetWorkerAsFinishedAsync(worker.Id);
                            }
                            else
                            {
                                await _workerRegistry.SetWorkerAsFailedAsync(worker.Id, task.Exception);
                            }
                        }, worker);
                }

                var currentTasks = tasks.Values
                    .Where(x => x != null)
                    .ToArray();

                if (currentTasks.Any())
                {
                    var completedTask = await Task.WhenAny(currentTasks!);
                    var completedWorkerId = tasks.Single(x => completedTask.Equals(x.Value))
                        .Key;

                    _logger.LogWarning("A worker ran to completion ({workerId}).", completedWorkerId);
                }

                await Task.Delay(restartWorkerDelay, cancellationToken);
            }
        }

        private Task? StartWorkerAsync(WorkerRegistryEntry workerEntry, CancellationToken cancellationToken)
        {
            var scope = _services.CreateScope();

            var context = new WorkerContext(scope.ServiceProvider);

            var worker = (IWorker?)Activator.CreateInstance(workerEntry.WorkerType);

            if (worker == null)
            {
                _logger.LogWarning("Could not create worker ({workerType})", workerEntry.WorkerType);
                return Task.CompletedTask;
            }

            return worker
                .ExecuteAsync(context, cancellationToken)
                .ContinueWith((_, __) => scope.Dispose(), null, cancellationToken);
        }
    }
}
