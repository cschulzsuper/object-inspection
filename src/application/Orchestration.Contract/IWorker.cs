using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IWorker
    {
        Task ExecuteAsync(WorkerContext context, CancellationToken cancellationToken);
    }
}
