using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Inventory;
using Super.Paula.Environment;

namespace Super.Paula.Data.Mapping.PartitionKeyValueGenerators
{
    internal class BusinessObjectPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObject>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState, 
                (entry.Entity as BusinessObject)!);

        public string Value(AppState appState, BusinessObject entity)
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : appState.CurrentOrganization;


        public string Value(AppState appState, Queue<object> partitionKeyComponents)
            => appState.IgnoreCurrentOrganization
                ? string.Empty
                : appState.CurrentOrganization;

    }
}