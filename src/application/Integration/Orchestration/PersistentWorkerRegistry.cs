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

        public bool Empty()
            => !_workerRegistrations.Any();

        public async IAsyncEnumerable<WorkerRegistration> GetUnstartedWorkerAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            foreach (var workerRegistration in _workerRegistrations.Values)
            {
                var workerManager = scope.ServiceProvider.GetRequiredService<IWorkerManager>();

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
                else
                {
                    if (unstartedWorker.State == "started" &&
                        unstartedWorker.ETag == workerRegistration.ETag)
                    {
                        continue;
                    }

                    unstartedWorker.State = string.Empty;

                    await workerManager.UpdateAsync(unstartedWorker);

                    workerRegistration.IterationDelay = unstartedWorker.IterationDelay;
                }

                yield return workerRegistration;
            }
        }

        public async ValueTask SetWorkerAsStartedAsync(string workerName)
        {
            using var scope = _serviceProvider.CreateScope();

            var workerManager = scope.ServiceProvider.GetRequiredService<IWorkerManager>();

            var unstartedWorker = workerManager
                .GetQueryable()
                .SingleOrDefault(x =>
                    x.UniqueName == workerName &&
                    x.State != "started");

            if (unstartedWorker != null)
            {
                unstartedWorker.State = "started";
                await workerManager.UpdateAsync(unstartedWorker);
                _workerRegistrations[unstartedWorker.UniqueName].ETag = unstartedWorker.ETag;

                _logger.LogInformation("A worker has been marked as started ({worker})", unstartedWorker);
            }
            else
            {
                _logger.LogWarning("An unkown worker has been marked as started ({workerName})", workerName);
            }
        }

        public async ValueTask SetWorkerAsFinishedAsync(string workerName)
        {
            using var scope = _serviceProvider.CreateScope();

            var workerManager = scope.ServiceProvider.GetRequiredService<IWorkerManager>();

            var startedWorker = workerManager
                .GetQueryable()
                .SingleOrDefault(x =>
                    x.UniqueName == workerName &&
                    x.State == "started");

            if (startedWorker != null)
            {
                startedWorker.State = "finished";
                await workerManager.UpdateAsync(startedWorker);
                _workerRegistrations[startedWorker.UniqueName].ETag = startedWorker.ETag;

                _logger.LogInformation("A worker has been marked as finished ({worker})", startedWorker);
            }
            else
            {
                _logger.LogWarning("An unkown worker has been marked as finished ({workerName})", workerName);
            }
        }

        public async ValueTask SetWorkerAsFailedAsync(string workerName, Exception? exception)
        {
            using var scope = _serviceProvider.CreateScope();

            var workerManager = scope.ServiceProvider.GetRequiredService<IWorkerManager>();

            var startedWorker = workerManager
                .GetQueryable()
                .SingleOrDefault(x =>
                    x.UniqueName == workerName &&
                    x.State == "started");

            if (startedWorker != null)
            {
                startedWorker.State = "failed";
                await workerManager.UpdateAsync(startedWorker);
                _workerRegistrations[startedWorker.UniqueName].ETag = startedWorker.ETag;

                _logger.LogWarning(exception, "An worker has been marked as failed ({worker})", startedWorker);
            }
            else
            {
                _logger.LogWarning(exception, "An unkown worker has been marked as failed ({workerName})", workerName);
            }
        }
    }
}
