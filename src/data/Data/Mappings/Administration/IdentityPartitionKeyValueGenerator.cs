using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Administration;
using Super.Paula.Environment;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Administration
{
    internal class IdentityPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Identity>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as Identity)!);

        public string Value(AppState appState, Identity entity)
            => "default";

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
            => "default";
    }
}