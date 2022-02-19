using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Administration;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Administration
{
    public class OrganizationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Organization>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as Organization)!);

        public string Value(PaulaContextState state, Organization entity)
            => "default";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => "default";
    }
}