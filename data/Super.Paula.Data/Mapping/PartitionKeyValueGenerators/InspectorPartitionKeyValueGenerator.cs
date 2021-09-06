using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Aggregates.Inspectors;
using Super.Paula.Environment;

namespace Super.Paula.Data.Mapping.PartitionKeyValueGenerators
{
    internal class InspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Inspector>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as Inspector)!);

        public string Value(AppState appState, Inspector entity)
        {
            var organization = appState.CurrentOrganization;

            return $"{organization}";
        }

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
        {
            var organization = appState.CurrentOrganization;

            return $"{organization}";
        }
    }
}