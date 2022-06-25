using Super.Paula.BlobStorage;
using System.IO;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class InspectorAvatarManager : IInspectorAvatarManager
    {
        private readonly IBlobStorage _blobStorage;

        public InspectorAvatarManager(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async ValueTask<Stream> GetAsync(string path)
            => await _blobStorage.ReadAsync($"inspector-avatars/{path}");

        public async ValueTask DeleteAsync(string path)
            => await _blobStorage.RemoveAsync($"inspector-avatars/{path}");

        public async ValueTask SetAsync(Stream stream, string path)
            => await _blobStorage.WriteOrReplaceAsync(new InspectorAvatarStream(stream), $"inspector-avatars/{path}");

    }
}
