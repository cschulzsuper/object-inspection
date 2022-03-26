using System;

namespace Super.Paula.RuntimeData
{
    public interface IRuntimeCache<TEntity>
        where TEntity : class, IRuntimeData
    {
        TEntity? GetOrDefault(params string[] correlationParts);

        TEntity GetOrCreate(Func<TEntity> create, params string[] correlationParts);

        TEntity GetAndUpdate(Action<TEntity> update, params string[] correlationParts);

        TEntity CreateOrUpdate(Func<TEntity> create, Action<TEntity> update, params string[] correlationParts);

        void Remove(params string[] correlationParts);

        void Set(TEntity value);
    }
}
