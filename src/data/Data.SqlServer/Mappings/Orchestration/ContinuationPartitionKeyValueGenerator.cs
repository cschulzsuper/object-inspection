using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;

public class ContinuationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Continuation>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Continuation)!);

    public string Value(ObjectInspectionContextState state, Continuation entity)
        => $"continuation";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"continuation";
}