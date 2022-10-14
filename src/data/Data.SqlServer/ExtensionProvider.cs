using Microsoft.Extensions.DependencyInjection;
using ChristianSchulz.ObjectInspection.Application.Operation;
using System;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Data;

public class ExtensionProvider
{
    private readonly IServiceProvider _services;
    private readonly ExtensionCache _extensionCache;
    private readonly ExtensionCacheKeyFactory _extensionCacheKeyFactory;

    public ExtensionProvider(
        IServiceProvider services,
        ExtensionCache extensionCache,
        ExtensionCacheKeyFactory extensionCacheKeyFactory)
    {
        _services = services;
        _extensionCache = extensionCache;
        _extensionCacheKeyFactory = extensionCacheKeyFactory;
    }

    public Extension? Get(string aggregateType)
    {
        var extensionCacheKey = _extensionCacheKeyFactory.Create(aggregateType);

        if (_extensionCache.TryGetValue(extensionCacheKey, out var value))
        {
            return value;
        };

        var extension = _services
                .GetRequiredService<ObjectInspectionContexts>().Operation
                .Set<Extension>()
                .SingleOrDefault(x => x.AggregateType == aggregateType);

        if (extension != null)
        {
            _extensionCache[extensionCacheKey] = extension;
        }

        return extension;
    }
}