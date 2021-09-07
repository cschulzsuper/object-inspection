using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Aggregates.Administration;
using Super.Paula.Environment;

namespace Super.Paula.Data.Mapping.PartitionKeyValueGenerators
{
    internal class OrganizationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Organization>
    {
         public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as Organization)!);

        public string Value(AppState appState, Organization entity)
            => "default";

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
            => "default";
    }
}