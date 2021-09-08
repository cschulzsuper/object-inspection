using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Aggregates.Auditing;
using Super.Paula.Data.Mapping.PartitionKeyValueGenerators;

namespace Super.Paula.Data.Mapping
{
    public class BusinessObjectInspectionAuditMapping : IEntityTypeConfiguration<BusinessObjectInspectionAudit>
    {
        public string PartitionKey = nameof(PartitionKey);

        public void Configure(EntityTypeBuilder<BusinessObjectInspectionAudit> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<BusinessObjectInspectionAuditPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, 
                    nameof(BusinessObjectInspectionAudit.BusinessObject),
                    nameof(BusinessObjectInspectionAudit.Inspection),
                    nameof(BusinessObjectInspectionAudit.AuditTime));

            builder
                .ToContainer(nameof(BusinessObjectInspectionAudit))
                .HasPartitionKey(PartitionKey)
                .HasNoDiscriminator();

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
