using Microsoft.Extensions.Hosting;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Server.Orchestration;

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