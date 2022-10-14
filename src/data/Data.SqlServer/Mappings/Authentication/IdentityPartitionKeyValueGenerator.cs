using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Authentication;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Authentication;

public class IdentityPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Identity>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Identity)!);

    public string Value(ObjectInspectionContextState state, Identity entity)
        => "default";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => "default";
}