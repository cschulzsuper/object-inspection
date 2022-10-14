using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Administration;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Administration;

public class IdentityInspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<IdentityInspector>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as IdentityInspector)!);

    public string Value(ObjectInspectionContextState state, IdentityInspector entity)
        => $"identity-inspector|{entity.UniqueName}";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"identity-inspector|{partitionKeyComponents.Dequeue()}";
}