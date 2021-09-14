using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Administration;
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
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : appState.CurrentOrganization;

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : appState.CurrentOrganization;
    }
}