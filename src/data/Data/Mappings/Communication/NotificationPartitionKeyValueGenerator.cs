using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Communication;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Communication
{
    public class NotificationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Notification>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as Notification)!);

        public string Value(PaulaContextState state, Notification entity)
            => $"notification/{entity.Inspector}";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => $"notification/{partitionKeyComponents.Dequeue()}";
    }
}