using System.Threading;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Orchestration;

public interface IWorkerHost
{
    Task StartAllWorkerAsync(CancellationToken cancellationToken);
}