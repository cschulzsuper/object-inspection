using System.IO;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration;

public interface IInspectorAvatarRequestHandler
{
    ValueTask<Stream> ReadAsync(string inspector);

    ValueTask WriteAsync(Stream stream, string inspector);

    ValueTask DeleteAsync(string inspector);
}