using Microsoft.Extensions.Logging;
using Super.Paula.Application.Administration.Exceptions;
using Super.Paula.Application.Guidelines;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class InspectorAvatarHandler : IInspectorAvatarHandler
    {
        private readonly ILogger<InspectorAvatarHandler> _logger;
        private readonly IInspectorAvatarManager _inspectorAvatarManager;

        public InspectorAvatarHandler(
            ILogger<InspectorAvatarHandler> logger,
            IInspectorAvatarManager inspectorAvatarManager)
        {
            _logger = logger;
            _inspectorAvatarManager = inspectorAvatarManager;
        }

        public async ValueTask DeleteAsync(string inspector)
{
            await _inspectorAvatarManager.DeleteAsync($"{inspector}");
        }

        public async ValueTask<Stream> ReadAsync(string inspector)
        {
            try
            {
                return await _inspectorAvatarManager.GetAsync($"{inspector}");
            }
            catch (InspectorAvatarNotFoundException exception)
            {
                _logger.LogTrace(exception, "Could not get inspector avatar '{uniqueName}'.", inspector);

                var @default = ReadDefault();

                if (@default == null)
                {
                    throw;
                }

                return @default;
            }
        }

        private static Stream? ReadDefault()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{typeof(InspectorAvatarHandler).Namespace}.Resources.inspector-avatar.jpg";

            return assembly.GetManifestResourceStream(resourceName);
        }

        public async ValueTask WriteAsync(Stream stream, string inspector)
        {
            await _inspectorAvatarManager.SetAsync(stream, $"{inspector}");
        }
    }
}
