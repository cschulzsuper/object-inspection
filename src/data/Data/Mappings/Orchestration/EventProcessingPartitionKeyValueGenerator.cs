using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Orchestration;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Orchestration
{
    public class EventProcessingPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<EventProcessing>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as EventProcessing)!);

        public string Value(PaulaContextState state, EventProcessing entity)
            => $"event-processing";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => $"event-processing";
    }
}