using System.Threading.Tasks;

namespace Super.Paula.Application.Operation
{
    public interface IApplicationManager
    {
        ValueTask InitializeAsync();
        ValueTask PrugeAsync();
    }
}
