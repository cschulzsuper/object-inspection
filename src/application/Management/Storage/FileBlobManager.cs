using Super.Paula.RuntimeData;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Super.Paula.Application.Storage
{
    public class FileBlobManager : IFileBlobManager
    {
        public record FileBlob(string Path) : IRuntimeData
        {
            public string BTag { get; set; } = null!;

            public byte[] Data { get; set; } = null!;

            public string Correlation => Path;
        }

        private readonly IRuntimeCache<FileBlob> _fileBlobRuntimeCache;

        public FileBlobManager(IRuntimeCache<FileBlob> fileBlobRuntimeCache)
        {
            _fileBlobRuntimeCache = fileBlobRuntimeCache;
        }

        public ValueTask<Stream> ReadAsync(string path)
        {
            var entity = _fileBlobRuntimeCache.GetOrDefault(path);

            if (entity == default)
            {
                throw new ManagementException($"File blob {path} not found.");
            }

            
            return ValueTask.FromResult((Stream)new MemoryStream(entity.Data));
        }

        public ValueTask RemoveAsync(string path, string btag)
        {
            var entity = _fileBlobRuntimeCache.GetOrDefault(path);

            if (entity == default)
            {
                throw new ManagementException($"File blob {path} not found.");
            }

#if false
            if (entity.BTag != btag)
            {
                throw new ManagementException($"File blob {path} btag {btag} invalid.");
            }
#endif

            _fileBlobRuntimeCache.Remove(path);

            return ValueTask.CompletedTask;
        }

        public async ValueTask<string> ReplaceAsync(Stream stream, string path, string btag)
        {
            var entity = _fileBlobRuntimeCache.GetOrDefault(path);

            if (entity == default)
            {
                throw new ManagementException($"File blob {path} not found.");
            }
#if false
            if (entity.BTag != btag)
            {
                throw new ManagementException($"File blob {path} btag {btag} invalid.");
            }
#endif

            var newTag = Guid.NewGuid().ToString();

            var dataStream = new MemoryStream();
            await stream.CopyToAsync(dataStream);;

            _fileBlobRuntimeCache.CreateOrUpdate(
                () => new FileBlob(path)
                {
                    BTag = newTag,
                    Data = dataStream.ToArray(),
                },
                x =>
                {
                    x.BTag = newTag;
                    x.Data = dataStream.ToArray();
                });

            return newTag;
        }

        public async ValueTask<string> WriteAsync(Stream stream, string path)
        {
            var entity = _fileBlobRuntimeCache.GetOrDefault(path);

            if (entity != default)
            {
                throw new ManagementException($"File blob {path} exists.");
            }

            var newTag = Guid.NewGuid().ToString();

            var dataStream = new MemoryStream();
            await stream.CopyToAsync(dataStream);

            _fileBlobRuntimeCache.Set(
                new FileBlob(path)
                {
                    BTag = newTag,
                    Data = dataStream.ToArray(),
                });

            return newTag;
        }
    }
}
