using System.Runtime.CompilerServices;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Data.Mappings;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Data;

public sealed class ExtensionRepository : Repository<Extension>
{
    private readonly ExtensionCache _extensionCache;
    private readonly ExtensionCacheKeyFactory _extensionCacheKeyFactory;

    public ExtensionRepository(
        ObjectInspectionContexts objectInspectionContexts,
        ObjectInspectionContextState objectInspectionContextState,
        IPartitionKeyValueGenerator<Extension> partitionKeyValueGenerator,
        ExtensionCache extensionCache,
        ExtensionCacheKeyFactory extensionCacheKeyFactory)

        : base(objectInspectionContexts.Operation, objectInspectionContextState, partitionKeyValueGenerator)
    {
        _extensionCache = extensionCache;
        _extensionCacheKeyFactory = extensionCacheKeyFactory;
    }

    public override async ValueTask<Extension> GetByIdAsync(object id)
    {
        GuardAggregateType(id);

        var extension = GetExtensionFromExtensionCacheOrDefault((string)id);
        return extension ?? await base.GetByIdAsync(id);
    }

    public override async ValueTask<Extension?> GetByIdOrDefaultAsync(object id)
    {
        GuardAggregateType(id);

        var extension = GetExtensionFromExtensionCacheOrDefault((string)id);
        return extension ?? await base.GetByIdOrDefaultAsync(id);
    }

    public override async ValueTask<Extension> GetByIdsAsync(params object[] ids)
    {
        GuardAggregateType(ids);

        var extension = GetExtensionFromExtensionCacheOrDefault((string)ids[0]);
        return extension ?? await base.GetByIdsAsync(ids);
    }

    public override Extension? GetByIdsOrDefault(params object[] ids)
    {
        GuardAggregateType(ids);

        var extension = GetExtensionFromExtensionCacheOrDefault((string)ids[0]);
        return extension ?? base.GetByIdsOrDefault(ids);
    }

    public override async ValueTask<Extension?> GetByIdsOrDefaultAsync(params object[] ids)
    {
        GuardAggregateType(ids);

        var extension = GetExtensionFromExtensionCacheOrDefault((string)ids[0]);
        return extension ?? await base.GetByIdsOrDefaultAsync(ids);
    }

    public override async ValueTask InsertAsync(Extension entity)
    {
        await base.InsertAsync(entity);

        RemoveExtensionFromExtensionCache(entity);
    }


    public override async ValueTask UpdateAsync(Extension entity)
    {
        await base.UpdateAsync(entity);

        RemoveExtensionFromExtensionCache(entity);
    }

    public override async ValueTask DeleteAsync(Extension entity)
    {
        await base.DeleteAsync(entity);

        RemoveExtensionFromExtensionCache(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void GuardAggregateType(object id)
    {
        if (id is not string and not (object[] and [string]))
        {
            throw new RepositoryException($"The id represents the aggregate type, it must be a single item of type string.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Extension? GetExtensionFromExtensionCacheOrDefault(string aggregateType)
    {
        var extensionCacheKey = _extensionCacheKeyFactory.Create(aggregateType);
        
        if (_extensionCache.TryGetValue(extensionCacheKey, out var extension))
        {
            return extension;
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RemoveExtensionFromExtensionCache(Extension entity)
    {
        var extensionCacheKey = _extensionCacheKeyFactory.Create(entity.AggregateType);
        _extensionCache[extensionCacheKey] = null;
    }

}