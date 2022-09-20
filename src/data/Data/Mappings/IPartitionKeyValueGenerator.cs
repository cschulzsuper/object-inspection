using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings;

public interface IPartitionKeyValueGenerator<TEntity>
{
    string Value(ObjectInspectionContextState appState, TEntity entity);

    string Value(ObjectInspectionContextState appState, Queue<object> partitionKeyComponents);
}