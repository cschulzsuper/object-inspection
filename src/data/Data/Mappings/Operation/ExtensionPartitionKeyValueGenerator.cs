using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Operation;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Operation;

public class ExtensionPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Extension>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as PaulaContext)!.State,
            (entry.Entity as Extension)!);

    public string Value(PaulaContextState state, Extension entity)
        => $"extension";

    public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
        => $"extension";

}