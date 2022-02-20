using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Inventory;

namespace Super.Paula.Data.Mappings.Inventory
{
    public class BusinessObjectMapping : IEntityTypeConfiguration<BusinessObject>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;

        public BusinessObjectMapping(PaulaContextState state)
        {
            _state = state;
        }

        public void Configure(EntityTypeBuilder<BusinessObject> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<BusinessObjectPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(BusinessObject.UniqueName));

            builder
                .ToContainer(_state.CurrentOrganization)
                .HasPartitionKey(PartitionKey)
                .HasDiscriminator<string>("Type");

            builder
                .Property(x => x.UniqueName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.DisplayName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Inspector)
                .HasMaxLength(140)
                .IsRequired();

            var inspectionsBuilder = builder
                .OwnsMany(x => x.Inspections);

            inspectionsBuilder
                .Property(x => x.Activated)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.AuditAnnotation)
                .HasMaxLength(4000)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.AssignmentDate)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.AssignmentTime)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.AuditDate)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.AuditTime)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.AuditInspector)
                .HasMaxLength(140)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.AuditResult)
                .HasMaxLength(140)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.UniqueName)
                .HasMaxLength(140)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.DisplayName)
                .HasMaxLength(140)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.Text)
                .HasMaxLength(4000)
                .IsRequired();

            var auditScheduleBuilder = inspectionsBuilder
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
