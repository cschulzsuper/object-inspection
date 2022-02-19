using System.Collections.Generic;

namespace Super.Paula.Data.Mappings
{
    public interface IPartitionKeyValueGenerator<TEntity>
    {
        string Value(PaulaContextState appState, TEntity entity);

        string Value(PaulaContextState appState, Queue<object> partitionKeyComponents);
    }
}
