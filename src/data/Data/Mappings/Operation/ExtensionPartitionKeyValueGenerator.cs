using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Operation;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Operation;

public class ExtensionPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Extension>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Extension)!);

    public string Value(ObjectInspectionContextState state, Extension entity)
        => $"extension";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"extension";

}