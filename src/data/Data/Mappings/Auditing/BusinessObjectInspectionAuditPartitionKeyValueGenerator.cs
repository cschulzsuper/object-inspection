using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Super.Paula.Application.Auditing;
using Super.Paula.Environment;
using System.Collections.Generic;

namespace Super.Paula.Data.Mappings.Auditing
{
    internal class BusinessObjectInspectionAuditPartitionKeyValueGenerator : ValueGenerator<string>, IPartitionKeyValueGenerator<BusinessObjectInspectionAudit>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
            => Value(
                (entry.Context as PaulaContext)!.AppState,
                (entry.Entity as BusinessObjectInspectionAudit)!);

        public string Value(AppState appState, BusinessObjectInspectionAudit entity)
        {
            var organization = appState.CurrentOrganization;
            var auditDate = entity.AuditDate;

            return $"{organization}/{auditDate}";
        }

        public string Value(AppState appState, Queue<object> partitionKeyComponents)
        {
            var organization = appState.CurrentOrganization;
            var auditDate = partitionKeyComponents.Dequeue();

            return $"{organization}/{auditDate}";
        }
    }
}