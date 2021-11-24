using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Administration;
using Super.Paula.Environment;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Administration
{
    internal class InspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Inspector>
    {
        private const string Version = "1";

        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as Inspector)!);

        public string Value(AppState appState, Inspector entity)
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : $"{Version}/{appState.CurrentOrganization}";

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : $"{Version}/{appState.CurrentOrganization}";
    }
}