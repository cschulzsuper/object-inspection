using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;

public class EventPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Event>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Event)!);

    public string Value(ObjectInspectionContextState state, Event entity)
        => $"event";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"event";
}