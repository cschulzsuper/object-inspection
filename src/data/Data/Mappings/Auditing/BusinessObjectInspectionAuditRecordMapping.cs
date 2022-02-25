using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Auditing;

namespace Super.Paula.Data.Mappings.Auditing
{
    public class BusinessObjectInspectionAuditRecordMapping : IEntityTypeConfiguration<BusinessObjectInspectionAuditRecord>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;

        public BusinessObjectInspectionAuditRecordMapping(PaulaContextState state)
        {
            _state = state;
        }

        public void Configure(EntityTypeBuilder<BusinessObjectInspectionAuditRecord> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<BusinessObjectInspectionAuditRecordPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey,
                    nameof(BusinessObjectInspectionAuditRecord.BusinessObject),
                    nameof(BusinessObjectInspectionAuditRecord.Inspection),
                    nameof(BusinessObjectInspectionAuditRecord.AuditTime));

            builder
                .ToContainer(_state.CurrentOrganization)
                .HasPartitionKey(PartitionKey)
                .HasDiscriminator<string>("Type");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

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
                .Property(x => x.Inspector)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Result)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.AuditDate)
                .IsRequired();

            builder
                .Property(x => x.AuditTime)
                .IsRequired();

            builder
                .Property(x => x.Annotation)
                .HasMaxLength(4000)
                .IsRequired();

        }
    }
}
