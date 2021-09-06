using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Aggregates.InspectionBusinessObjects;
using Super.Paula.Environment;

namespace Super.Paula.Data.Mapping.PartitionKeyValueGenerators
{
    internal class InspectionBusinessObjectPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<InspectionBusinessObject>
    {

        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as InspectionBusinessObject)!);

        public string Value(AppState appState, InspectionBusinessObject entity)
        {
            var organization = appState.CurrentOrganization;
            var inspection = entity.Inspection;

            return $"{organization}/{inspection}";
        }

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
        {
            var organization = appState.CurrentOrganization;
            var inspection = partitionKeyComponents.Dequeue();

            return $"{organization}/{inspection}";
        }
    }
}