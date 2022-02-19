using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Guidelines;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Guidelines
{
    public class InspectionPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Inspection>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as Inspection)!);

        public string Value(PaulaContextState state, Inspection entity)
            => state.IgnoreCurrentOrganization
                ? string.Empty
                : state.CurrentOrganization;


        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => state.IgnoreCurrentOrganization
                ? string.Empty
                : state.CurrentOrganization;

    }
}