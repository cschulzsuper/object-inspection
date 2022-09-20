using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;

public class EventProcessingPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<EventProcessing>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as EventProcessing)!);

    public string Value(ObjectInspectionContextState state, EventProcessing entity)
        => $"event-processing";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"event-processing";
}