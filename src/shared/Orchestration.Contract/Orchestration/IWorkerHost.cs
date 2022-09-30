using System.Threading;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Shared.Orchestration;

public interface IWorkerHost
{
    Task StartAllWorkerAsync(CancellationToken cancellationToken);
}