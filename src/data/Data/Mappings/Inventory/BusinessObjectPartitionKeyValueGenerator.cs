using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Inventory;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Inventory
{
    public class BusinessObjectPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObject>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as BusinessObject)!);

        public string Value(PaulaContextState state, BusinessObject entity)
            => "business-object";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => "business-object";

    }
}