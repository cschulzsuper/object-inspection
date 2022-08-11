using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
    public interface IApplicationManager
    {
        ValueTask InitializeAsync(string organization);

        ValueTask PrugeAsync(string organization);
    }
}
