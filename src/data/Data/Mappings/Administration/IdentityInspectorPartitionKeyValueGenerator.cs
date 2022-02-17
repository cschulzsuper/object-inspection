using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Administration;
using Super.Paula.Environment;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Administration
{
    public class IdentityInspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<IdentityInspector>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as IdentityInspector)!);

        public string Value(AppState appState, IdentityInspector entity)
            => entity.UniqueName;

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
        {
            var identity = partitionKeyComponents.Dequeue();
            return $"{identity}";
        }
    }
}