using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;

public class BusinessObjectInspectionPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspection>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as BusinessObjectInspection)!);

    public string Value(ObjectInspectionContextState state, BusinessObjectInspection entity)
        => $"business-object-inspection|{entity.BusinessObject}";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"business-object-inspection|{partitionKeyComponents.Dequeue()}";

}