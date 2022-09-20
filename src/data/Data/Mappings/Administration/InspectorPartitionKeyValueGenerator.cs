using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Administration;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Administration;

public class InspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Inspector>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Inspector)!);

    public string Value(ObjectInspectionContextState state, Inspector entity)
        => "inspector";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => "inspector";
}