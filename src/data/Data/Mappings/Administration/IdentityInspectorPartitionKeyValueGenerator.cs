using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Administration;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Administration
{
    public class IdentityInspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<IdentityInspector>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as IdentityInspector)!);

        public string Value(PaulaContextState state, IdentityInspector entity)
            => $"identity-inspector|{entity.UniqueName}";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => $"identity-inspector|{partitionKeyComponents.Dequeue()}";
    }
}