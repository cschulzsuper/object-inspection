using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Communication;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Communication;

public class NotificationPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<Notification>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as Notification)!);

    public string Value(ObjectInspectionContextState state, Notification entity)
        => $"notification|{entity.Inspector}";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"notification|{partitionKeyComponents.Dequeue()}";
}