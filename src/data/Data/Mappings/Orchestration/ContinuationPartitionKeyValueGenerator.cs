using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Orchestration;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Orchestration;

public class ContinuationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Continuation>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as PaulaContext)!.State,
            (entry.Entity as Continuation)!);

    public string Value(PaulaContextState state, Continuation entity)
        => $"continuation";

    public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
        => $"continuation";
}