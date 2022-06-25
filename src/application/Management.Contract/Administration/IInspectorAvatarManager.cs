using System.IO;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public interface IInspectorAvatarManager
    {
        ValueTask<Stream> GetAsync(string path);

        ValueTask SetAsync(Stream stream, string path);

        ValueTask DeleteAsync(string path);
    }
}
