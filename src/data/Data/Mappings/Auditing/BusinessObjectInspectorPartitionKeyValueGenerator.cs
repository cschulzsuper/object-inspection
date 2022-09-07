using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Auditing;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Auditing;

public class BusinessObjectInspectorPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspector>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as PaulaContext)!.State,
            (entry.Entity as BusinessObjectInspector)!);

    public string Value(PaulaContextState state, BusinessObjectInspector entity)
        => $"business-object-inspector|{entity.BusinessObject}";

    public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
        => $"business-object-inspector|{partitionKeyComponents.Dequeue()}";

}