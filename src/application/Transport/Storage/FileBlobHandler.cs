using Super.Paula.Application.Storage.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Application.Storage
{
    public class FileBlobHandler : IFileBlobHandler
    {
        private readonly IFileBlobManager _fileBlobManager;

        public FileBlobHandler(IFileBlobManager fileBlobManager)
        {
            _fileBlobManager = fileBlobManager;
        }

        public async ValueTask<Stream> ReadAsync(string container, string uniqueName)
        {
            return await _fileBlobManager.ReadAsync($"{container}/{uniqueName}");
        }

        public async ValueTask<FileBlobResponse> WriteAsync(Stream stream, string container)
        {
            var uniqueName = Guid.NewGuid().ToString();

            var btag = await _fileBlobManager.WriteAsync(stream, $"{container}/{uniqueName}");

            return new FileBlobResponse
            {
                BTag = btag,
                UniqueName = uniqueName
            };
        }

        public async ValueTask<FileBlobResponse> WriteAsync(Stream stream, string container, string uniqueName, string? btag)
        {
            if (btag != null)
            {
                btag = await _fileBlobManager.ReplaceAsync(stream, $"{container}/{uniqueName}", btag);
            }
            else
            {
                btag = await _fileBlobManager.WriteAsync(stream, $"{container}/{uniqueName}");
            }

            return new FileBlobResponse
            {
                BTag = btag,
                UniqueName = uniqueName
            };
        }

        public async ValueTask DeleteAsync(string container, string uniqueName, string btag)
        {
            await _fileBlobManager.RemoveAsync($"{container}/{uniqueName}", btag);
        }
    }
}
