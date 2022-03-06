using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public class InMemoryWorkerRegistry : IWorkerRegistry
    {
        private readonly ConcurrentDictionary<Guid, WorkerRegistryEntry> _workers;
        private readonly ILogger<InMemoryWorkerRegistry> _logger;

        public InMemoryWorkerRegistry(ILogger<InMemoryWorkerRegistry> logger)
        {
            _workers = new ConcurrentDictionary<Guid, WorkerRegistryEntry>();
            _logger  =logger;
        }

        public void Register<TWorker>()
        {
            var workerEntry = new WorkerRegistryEntry(typeof(TWorker));

            _workers.AddOrUpdate(workerEntry.Id, workerEntry, (_, _) => workerEntry);
        }

        public bool HasUnstartedWorker()
        { 
            return !_workers.IsEmpty;
        }

        public async IAsyncEnumerable<WorkerRegistryEntry> GetUnstartedWorkerAsync()
        {
            await Task.CompletedTask;

            foreach (var worker in _workers.Values)
            {
                yield return worker;
            }
        }

        public ValueTask SetWorkerAsStartedAsync(Guid workerId)
        {
            var found = _workers.TryRemove(workerId, out var worker);

            if (found && worker != null)
            {
                _logger.LogInformation("A worker has been marked as started ({workerType})", worker.WorkerType);
            }
            else
            {
                _logger.LogWarning("An unkown worker has been marked as started ({workerId})", workerId);
            }

            return ValueTask.CompletedTask;
        }

        public ValueTask SetWorkerAsFinishedAsync(Guid workerId)
        {
            _logger.LogInformation("A worker has been marked as failed ({workerId})", workerId);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetWorkerAsFailedAsync(Guid workerId, Exception? exception)
        {
            _logger.LogWarning(exception, "An worker has been marked as failed ({workerId})", workerId);
            return ValueTask.CompletedTask;
        }
    }
}
