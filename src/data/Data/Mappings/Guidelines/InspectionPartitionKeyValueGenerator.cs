using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Guidelines;
using Super.Paula.Environment;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Guidelines
{
    public class InspectionPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Inspection>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as Inspection)!);

        public string Value(AppState appState, Inspection entity)
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : appState.CurrentOrganization;


        public string Value(AppState appState, Queue<object> partitionKeyComponents)
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : appState.CurrentOrganization;

    }
}