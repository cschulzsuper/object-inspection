using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Auditing;
using Super.Paula.Application.Inventory;
using Super.Paula.Data.Mappings.Inventory;

namespace Super.Paula.Data.Mappings.Auditing
{
    public class BusinessObjectInspectionMapping : IEntityTypeConfiguration<BusinessObjectInspection>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;

        public BusinessObjectInspectionMapping(PaulaContextState state)
        {
            _state = state;
        }

        public void Configure(EntityTypeBuilder<BusinessObjectInspection> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<BusinessObjectInspectionPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(BusinessObjectInspection.Inspection));

            builder
                .ToContainer(_state.CurrentOrganization)
                .HasPartitionKey(PartitionKey)
                .HasDiscriminator<string>("Type");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

            builder
                .Property(x => x.Activated)
                .IsRequired();

            builder
                .Property(x => x.AssignmentDate)
                .IsRequired();

            builder
                .Property(x => x.AssignmentTime)
                .IsRequired();

            builder
                .Property(x => x.BusinessObject)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.BusinessObjectDisplayName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Inspection)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.InspectionDisplayName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.InspectionText)
                .HasMaxLength(4000)
                .IsRequired();

            var auditBuilder = builder
                .OwnsOne(x => x.Audit);

            auditBuilder
                .Property(x => x.Annotation)
                .HasMaxLength(4000)
                .IsRequired();

            auditBuilder
                .Property(x => x.AuditDate)
                .IsRequired();

            auditBuilder
                .Property(x => x.AuditTime)
                .IsRequired();

            auditBuilder
                .Property(x => x.Inspector)
                .HasMaxLength(140)
                .IsRequired();

            auditBuilder
                .Property(x => x.Result)
                .HasMaxLength(140)
                .IsRequired();

            var auditScheduleBuilder = builder
                .OwnsOne(x => x.AuditSchedule);

            auditScheduleBuilder
                .Property(x => x.Threshold)
                .IsRequired();

            var auditScheduleExpressionsBuilder = auditScheduleBuilder
                .OwnsMany(x => x.Expressions);

            auditScheduleExpressionsBuilder
                .Property(x => x.CronExpression)
                .HasMaxLength(140)
                .IsRequired();

            var auditScheduleOmissionsBuilder = auditScheduleBuilder
                .OwnsMany(x => x.Omissions);

            auditScheduleOmissionsBuilder
                .Property(x => x.PlannedAuditDate)
                .IsRequired();

            auditScheduleOmissionsBuilder
                .Property(x => x.PlannedAuditTime)
                .IsRequired();

            var auditScheduleAdditionalsBuilder = auditScheduleBuilder
                .OwnsMany(x => x.Additionals);

            auditScheduleAdditionalsBuilder
                .Property(x => x.PlannedAuditDate)
                .IsRequired();

            auditScheduleAdditionalsBuilder
                .Property(x => x.PlannedAuditTime)
                .IsRequired();

            var auditScheduleAppointmentsBuilder = auditScheduleBuilder
                .OwnsMany(x => x.Appointments);

            auditScheduleAppointmentsBuilder
                .Property(x => x.PlannedAuditDate)
                .IsRequired();

            auditScheduleAppointmentsBuilder
                .Property(x => x.PlannedAuditTime)
                .IsRequired();
        }
    }
}
