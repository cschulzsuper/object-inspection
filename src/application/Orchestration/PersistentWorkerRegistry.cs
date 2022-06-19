using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class PersistentWorkerRegistry : IWorkerRegistry
    {
        private readonly ILogger<PersistentWorkerRegistry> _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<string, WorkerRegistration> _workerRegistrations;

        public PersistentWorkerRegistry(
            ILogger<PersistentWorkerRegistry> logger, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            _workerRegistrations = new ConcurrentDictionary<string, WorkerRegistration>();
        }

        public void Register<TWorker>(Action<WorkerOptions> configure)
        {
            var options = new WorkerOptions();
            configure.Invoke(options);

            if (string.IsNullOrWhiteSpace(options.Name))
            {
                options.Name = Guid.NewGuid().ToString();
            }

            if (options.IterationDelay == default)
            {
                options.IterationDelay = 60_000; // 1 minute
            }

            var workerRegistration = new WorkerRegistration(typeof(TWorker), options.Name)
            {
                IterationDelay = options.IterationDelay,
            };

            _workerRegistrations.AddOrUpdate(workerRegistration.WorkerName, workerRegistration, (_, _) => workerRegistration);
        }

        public bool HasWorkers()
            => _workerRegistrations.Any();

        public bool HasUnstartedWorkers()
        {
            using var scope = _serviceProvider.CreateScope();

            var workerRuntimeManager = scope.ServiceProvider.GetRequiredService<IWorkerRuntimeManager>();

            return _workerRegistrations.Any(x => string.IsNullOrWhiteSpace(workerRuntimeManager.GetState(x.Key)));
        }

        public async IAsyncEnumerable<WorkerRegistration> GetUnstartedWorkerAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            foreach (var workerRegistration in _workerRegistrations.Values)
            {
                var workerManager = scope.ServiceProvider.GetRequiredService<IWorkerManager>();
                var workerRuntimeManager = scope.ServiceProvider.GetRequiredService<IWorkerRuntimeManager>();

                var workerState = workerRuntimeManager.GetState(workerRegistration.WorkerName);

                if (workerState != "starting" &&
                    workerState != "running")
                {
                    var unstartedWorker = workerManager
                        .GetQueryable()
                        .SingleOrDefault(x => x.UniqueName == workerRegistration.WorkerName);

                    if (unstartedWorker == null)
                    {
                        unstartedWorker = new Worker
                        {
                            UniqueName = workerRegistration.WorkerName,
                            IterationDelay = workerRegistration.IterationDelay
                        };

                        await workerManager.InsertAsync(unstartedWorker);
                    }

                    workerRegistration.IterationDelay = unstartedWorker.IterationDelay;

                    yield return workerRegistration;
                } 
            }
        }

        public async ValueTask<bool> SetWorkerAsStartingAsync(string workerName)
        {
            using var scope = _serviceProvider.CreateScope();

            var workerManager = scope.ServiceProvider.GetRequiredService<IWorkerManager>();
            var workerRuntimeManager = scope.ServiceProvider.GetRequiredService<IWorkerRuntimeManager>();

            var unstartedWorker = workerManager
                .GetQueryable()
                .SingleOrDefault(x => x.UniqueName == workerName);

            if (unstartedWorker != null)
            {
                var successful = workerRuntimeManager.TrySetState(workerName, "starting");
                if(successful)
                {
                    await workerManager.UpdateAsync(unstartedWorker);
                    _logger.LogInformation("A worker has been marked as starting ({worker})", unstartedWorker);
                    return true;
                } 
                else
                {
                    _logger.LogWarning("A worker is already starting ({worker})", unstartedWorker);
                    return false;
                }
            }
            else
            {
                _logger.LogWarning("An unkown worker has been marked as starting ({workerName})", workerName);
                return false;
            }
        }

        public ValueTask SetWorkerAsRunningAsync(string workerName)
        {
            using var scope = _serviceProvider.CreateScope();

            var workerRuntimeManager = scope.ServiceProvider.GetRequiredService<IWorkerRuntimeManager>();

            var successful = workerRuntimeManager.TrySetState(workerName, "running");
            if (successful)
            {
                _logger.LogTrace("A worker has been marked as running ({worker})", workerName);
            }
            else
            {
                _logger.LogWarning("Could not set worker to running ({worker})", workerName);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask SetWorkerAsFinishedAsync(string workerName)
        {
            using var scope = _serviceProvider.CreateScope();

            var workerRuntimeManager = scope.ServiceProvider.GetRequiredService<IWorkerRuntimeManager>();

            var successful = workerRuntimeManager.TrySetState(workerName, "finished");
            if (successful)
            {
                _logger.LogInformation("A worker has been marked as finished ({worker})", workerName);
            }
            else
            {
                _logger.LogWarning("A worker has already finished ({worker})", workerName);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask SetWorkerAsFailedAsync(string workerName, Exception? exception)
        {
            using var scope = _serviceProvider.CreateScope();

            var workerRuntimeManager = scope.ServiceProvider.GetRequiredService<IWorkerRuntimeManager>();

            var successful = workerRuntimeManager.TrySetState(workerName, "failed");
            if (successful)
            {
                _logger.LogWarning(exception, "An worker has been marked as failed ({worker})", workerName);
            }
            else
            {
                _logger.LogWarning(exception, "A worker has already failed ({worker})", workerName);
            }

            return ValueTask.CompletedTask;
        }
    }
}
