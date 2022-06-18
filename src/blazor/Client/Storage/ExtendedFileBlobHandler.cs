using Microsoft.Extensions.Logging;
using Super.Paula.Application.Storage;
using Super.Paula.Application.Storage.Exceptions;
using Super.Paula.Application.Storage.Responses;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Super.Paula.Client.Storage
{
    public sealed class ExtendedFileBlobHandler : IFileBlobHandler
    {
        private readonly ILogger<ExtendedFileBlobHandler> _logger;
        private readonly IFileBlobHandler _fileBlobHandler;

        public ExtendedFileBlobHandler(
            ILogger<ExtendedFileBlobHandler> logger,
            IFileBlobHandler fileBlobHandler)
        {
            _logger = logger;
            _fileBlobHandler = fileBlobHandler;
        }

        public async ValueTask<Stream> ReadAsync(string container, string uniqueName)
        {
            try
            {
                return await _fileBlobHandler.ReadAsync(container, uniqueName);
            }
            catch (FileBlobReadException exception)
            {
                _logger.LogWarning(exception, $"Could not read file blob.");

                return ReadDefault(container, uniqueName) ?? throw exception;
            }
        }

        private static Stream? ReadDefault(string container, string uniqueName)
        {
            if (container.StartsWith("inspectors/") && uniqueName == "avatar")
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"{typeof(ExtendedFileBlobHandler).Namespace}.Resources.inspector-avatar.jpg";

                return assembly.GetManifestResourceStream(resourceName);
            }

            return null;
        }

        public ValueTask<FileBlobResponse> WriteAsync(Stream stream, string container)
            => _fileBlobHandler.WriteAsync(stream, container);

        public ValueTask<FileBlobResponse> WriteAsync(Stream stream, string container, string uniqueName, string? btag)
            => _fileBlobHandler.WriteAsync(stream, container, uniqueName, btag);

        public ValueTask DeleteAsync(string container, string uniqueName, string btag)
            => _fileBlobHandler.DeleteAsync(container, uniqueName, btag);
    }
}
