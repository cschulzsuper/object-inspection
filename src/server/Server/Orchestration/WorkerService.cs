using Microsoft.Extensions.Hosting;
using Super.Paula.Application.Orchestration;
using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Orchestration
{
    public class WorkerService : BackgroundService
    {
        private readonly IWorkerHost _workerHost;

        public WorkerService(IWorkerHost workerHost)
        {
            _workerHost = workerHost;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _workerHost.StartAllWorkerAsync(stoppingToken);
        }
    }
}
