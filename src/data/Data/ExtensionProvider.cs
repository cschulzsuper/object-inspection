using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Setup;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Super.Paula.Data
{
    public class ExtensionProvider
    {
        private readonly IServiceProvider _services;
        private readonly PaulaContextState _state;
        private readonly ExtensionCache _cache;

        public ExtensionProvider(
            IServiceProvider services,
            PaulaContextState state,
            ExtensionCache cache)
        {
            _services = services;
            _state = state;
            _cache = cache;
        }

        public Extension? this[string type]
        {
            set
            {
                var extensionCacheKey = CreateKey(type);
                _cache[extensionCacheKey] = value;
            }
            get
            {
                var extensionCacheKey = CreateKey(type);

                if (_cache.TryGetValue(extensionCacheKey, out var value) && value != null)
                {
                    return value;
                };

                var extension = _services
                        .GetRequiredService<PaulaSetupContext>()
                        .Set<Extension>()
                        .SingleOrDefault(x => x.Type == type);

                if (extension != null)
                {
                    _cache[extensionCacheKey] = extension;
                }

                return extension;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string CreateKey(string type)
            => $"{_state.CurrentOrganization}|{type}";
    }
}
