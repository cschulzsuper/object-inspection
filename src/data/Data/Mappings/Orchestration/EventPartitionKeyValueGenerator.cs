using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Orchestration;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Orchestration
{
    public class EventPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Event>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as Event)!);

        public string Value(PaulaContextState state, Event entity)
            => $"event";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => $"event";
    }
}