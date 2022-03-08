using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Auth;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Auth
{
    public class IdentityPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Identity>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as Identity)!);

        public string Value(PaulaContextState state, Identity entity)
            => "default";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => "default";
    }
}