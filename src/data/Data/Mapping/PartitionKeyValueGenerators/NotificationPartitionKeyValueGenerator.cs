using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Communication;
using Super.Paula.Environment;

namespace Super.Paula.Data.Mapping.PartitionKeyValueGenerators
{
    internal class NotificationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Notification>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as Notification)!);

        public string Value(AppState appState, Notification entity)
        {
            var organization = appState.CurrentOrganization;
            var inspector = entity.Inspector;

            return $"{organization}/{inspector}";
        }

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
        {
            var organization = appState.CurrentOrganization;
            var inspector = partitionKeyComponents.Dequeue();

            return $"{organization}/{inspector}";
        }
    }
}