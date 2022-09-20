using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Operation;

public interface IApplicationManager
{
    ValueTask InitializeAsync(string organization);

    ValueTask PrugeAsync(string organization);
}