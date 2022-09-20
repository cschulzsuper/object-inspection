using System.IO;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Administration;

public interface IInspectorAvatarManager
{
    ValueTask<Stream> GetAsync(string inspector);

    ValueTask SetAsync(Stream stream, string inspector);

    ValueTask DeleteAsync(string inspector);
}