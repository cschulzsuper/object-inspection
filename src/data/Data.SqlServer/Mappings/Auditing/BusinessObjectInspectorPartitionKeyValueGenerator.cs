using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;

public class BusinessObjectInspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspector>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as BusinessObjectInspector)!);

    public string Value(ObjectInspectionContextState state, BusinessObjectInspector entity)
        => $"business-object-inspector|{entity.BusinessObject}";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"business-object-inspector|{partitionKeyComponents.Dequeue()}";

}