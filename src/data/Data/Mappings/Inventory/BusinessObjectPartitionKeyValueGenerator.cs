using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Inventory;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Inventory;

public class BusinessObjectPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObject>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as BusinessObject)!);

    public string Value(ObjectInspectionContextState state, BusinessObject entity)
        => "business-object";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => "business-object";

}