using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Auditing;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Auditing
{
    public class BusinessObjectInspectionAuditPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspectionAudit>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.State,
                (entry.Entity as BusinessObjectInspectionAudit)!);

        public string Value(PaulaContextState state, BusinessObjectInspectionAudit entity)
            => $"business-object-inspection-audit/{entity.AuditDate}";

        public string Value(PaulaContextState state, Queue<object> partitionKeyComponents)
            => $"business-object-inspection-audit/{partitionKeyComponents.Dequeue()}";
    }
}