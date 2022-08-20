using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public interface IWorker
{
    Task ExecuteAsync(WorkerContext context, CancellationToken cancellationToken);
}