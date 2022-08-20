using System;
using System.Collections.Generic;
using System.Threading;

namespace Super.Paula.RuntimeData;

public class RuntimeCache<TEntity> : IRuntimeCache<TEntity>
    where TEntity : class, IRuntimeData
{
    private readonly IDictionary<string, TEntity> _runtimeCache = new Dictionary<string, TEntity>();

    private readonly SemaphoreSlim _runtimeCacheAccess;

    public RuntimeCache()
    {
        _runtimeCache = new Dictionary<string, TEntity>();
        _runtimeCacheAccess = new(1);
    }

    public TEntity? GetOrDefault(params string[] correlationParts)
    {
        var correlation = string.Join(":", correlationParts);

        try
        {
            _runtimeCacheAccess.Wait();

            return _runtimeCache.ContainsKey(correlation)
                ? _runtimeCache[correlation]
                : null;
        }
        finally
        {
            _runtimeCacheAccess.Release();
        }
    }

    public TEntity GetOrCreate(Func<TEntity> create, params string[] correlationParts)
    {
        var correlation = string.Join(":", correlationParts);

        try
        {
            _runtimeCacheAccess.Wait();

            var entryExists = _runtimeCache.ContainsKey(correlation);

            if (!entryExists)
            {
                _runtimeCache[correlation] = create.Invoke();
            }

            return _runtimeCache[correlation];
        }
        finally
        {
            _runtimeCacheAccess.Release();
        }
    }

    public TEntity GetAndUpdate(Action<TEntity> update, params string[] correlationParts)
    {
        var correlation = string.Join(":", correlationParts);

        try
        {
            _runtimeCacheAccess.Wait();

            var entry = _runtimeCache[correlation];
            update.Invoke(entry);
            return entry;
        }
        finally
        {
            _runtimeCacheAccess.Release();
        }
    }

    public TEntity CreateOrUpdate(Func<TEntity> create, Action<TEntity> update, params string[] correlationParts)
    {
        var correlation = string.Join(":", correlationParts);

        try
        {
            _runtimeCacheAccess.Wait();

            var entryExists = _runtimeCache.ContainsKey(correlation);

            if (!entryExists)
            {
                _runtimeCache[correlation] = create.Invoke();
                return _runtimeCache[correlation];
            }
            else
            {
                var entry = _runtimeCache[correlation];
                update.Invoke(entry);
                return entry;
            }
        }
        finally
        {
            _runtimeCacheAccess.Release();
        }
    }


    public void Remove(params string[] correlationParts)
    {
        var correlation = string.Join(":", correlationParts);
        try
        {
            _runtimeCacheAccess.Wait();

            if (_runtimeCache.ContainsKey(correlation))
            {
                _runtimeCache.Remove(correlation, out _);
            }
        }
        finally
        {
            _runtimeCacheAccess.Release();
        }
    }

    public void Set(TEntity value)
    {
        _runtimeCache[value.Correlation] = value;
    }
}