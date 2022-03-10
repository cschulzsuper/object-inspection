using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Auditing;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Auditing
{
    public class BusinessObjectInspectionAuditRecordPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspectionAuditRecord>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as BusinessObjectInspectionAuditRecord)!);

        public string Value(PaulaContextState state, BusinessObjectInspectionAuditRecord entity)
            => $"business-object-inspection-audit-record|{entity.AuditDate}";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => $"business-object-inspection-audit-record|{partitionKeyComponents.Dequeue()}";
    }
}