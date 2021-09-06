using Super.Paula.Environment;

namespace Super.Paula.Data.Mapping.PartitionKeyValueGenerators
{
    public interface IPartitionKeyValueGenerator<TEntity>
    {
        string Value(AppState appState, TEntity entity);

        string Value(AppState appState, Queue<object> partitionKeyComponents);
    }
}
