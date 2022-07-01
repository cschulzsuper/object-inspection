using System.IO;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorAvatarHandler
    {
        ValueTask<Stream> ReadAsync(string uniqueName);

        ValueTask WriteAsync(Stream stream, string uniqueName);

        ValueTask DeleteAsync(string uniqueName);
    }
}
