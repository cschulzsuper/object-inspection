using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public interface IWorker
{
    Task ExecuteAsync(WorkerContext context, CancellationToken cancellationToken);
}