using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Guidelines;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Guidelines;

public class InspectionPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Inspection>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Inspection)!);

    public string Value(ObjectInspectionContextState state, Inspection entity)
        => "inspection";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => "inspection";

}