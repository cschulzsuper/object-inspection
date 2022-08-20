using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Auditing;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Auditing;

public class BusinessObjectInspectionPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspection>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as PaulaContext)!.State,
            (entry.Entity as BusinessObjectInspection)!);

    public string Value(PaulaContextState state, BusinessObjectInspection entity)
        => $"business-object-inspection|{entity.BusinessObject}";

    public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
        => $"business-object-inspection|{partitionKeyComponents.Dequeue()}";

}