using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Orchestration;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Orchestration
{
    public class WorkerPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Worker>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as Worker)!);

        public string Value(PaulaContextState state, Worker entity)
            => $"worker";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => $"worker";
    }
}