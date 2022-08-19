using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Operation;
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

        public Extension? Get(string aggregateType)
        {
            var extensionCacheKey = CreateKey(aggregateType);

            if (_cache.TryGetValue(extensionCacheKey, out var value) && value != null)
            {
                return value;
            };

            // TODO check if it exists

            var extension = _services
                    .GetRequiredService<PaulaContexts>().Operation
                    .Set<Extension>()
                    .SingleOrDefault(x => x.AggregateType == aggregateType);

            if (extension != null)
            {
                _cache[extensionCacheKey] = extension;
            }

            return extension;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string CreateKey(string type)
            => $"{_state.CurrentOrganization}|{type}";
    }
}
