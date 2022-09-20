using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;

public class WorkerPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Worker>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Worker)!);

    public string Value(ObjectInspectionContextState state, Worker entity)
        => $"worker";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"worker";
}