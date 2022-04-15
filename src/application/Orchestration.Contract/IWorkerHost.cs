using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Application.Orchestration
{
    public interface IWorkerHost
    {
        Task StartAllWorkerAsync(CancellationToken cancellationToken);
    }
}