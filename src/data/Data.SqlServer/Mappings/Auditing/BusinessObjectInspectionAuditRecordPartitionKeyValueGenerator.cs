using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;

public class BusinessObjectInspectionAuditRecordPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspectionAuditRecord>
{
    public override bool GeneratesTemporaryValues => false;

    public override string Next(EntityEntry entry)
        => Value(
            (entry.Context as ObjectInspectionContext)!.State,
            (entry.Entity as BusinessObjectInspectionAuditRecord)!);

    public string Value(ObjectInspectionContextState state, BusinessObjectInspectionAuditRecord entity)
        => $"business-object-inspection-audit-record|{entity.AuditDate}";

    public string Value(ObjectInspectionContextState state, Queue<object> partitionKeyComponents)
        => $"business-object-inspection-audit-record|{partitionKeyComponents.Dequeue()}";
}