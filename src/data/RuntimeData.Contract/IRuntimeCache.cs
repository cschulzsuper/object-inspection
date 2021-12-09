namespace Super.Paula.RuntimeData
{
    public interface IRuntimeCache<TEntity>
        where TEntity : class, IRuntimeData
    {
        TEntity? Get(params string[] correlationParts);

        void Remove(params string[] correlationParts);

        void Set(TEntity value);
    }
}
