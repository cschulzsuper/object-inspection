using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Administration;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Administration
{
    public class InspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Inspector>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as Inspector)!);

        public string Value(PaulaContextState state, Inspector entity)
            => "inspector";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => "inspector";
    }
}