using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Operation;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Operation;

public class DistinctionTypePartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<DistinctionType>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as DistinctionType)!);

    public string Value(ObjectInspectionContextState state, DistinctionType entity)
        => $"distinction-type";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"distinction-type";

}