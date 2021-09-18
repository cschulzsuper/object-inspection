using Super.Paula.Environment;
using System.Collections.Generic;

namespace Super.Paula.Data.Mapping.PartitionKeyValueGenerators
{
    public interface IPartitionKeyValueGenerator<TEntity>
    {
        string Value(AppState appState, TEntity entity);

        string Value(AppState appState, Queue<object> partitionKeyComponents);
    }
}
