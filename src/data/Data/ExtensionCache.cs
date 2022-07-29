using Super.Paula.Application.Setup;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Super.Paula.Data
{
    public class ExtensionCache
    {
        private readonly IDictionary<string, Extension> _cache = new Dictionary<string, Extension>();

        public Extension? this[string key]
        {
            set
            {
                if (value == null)
                {
                    _cache.Remove(key);
                }
                else
                {
                    _cache[key] = value;
                }
            }

            get => _cache[key];
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out Extension value)
            => _cache.TryGetValue(key, out value);
    }
}
