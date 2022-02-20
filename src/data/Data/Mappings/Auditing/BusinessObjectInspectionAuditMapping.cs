using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Auditing;

namespace Super.Paula.Data.Mappings.Auditing
{
    public class BusinessObjectInspectionAuditMapping : IEntityTypeConfiguration<BusinessObjectInspectionAudit>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;

        public BusinessObjectInspectionAuditMapping(PaulaContextState state)
        {
            _state = state;
        }

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
                .ToContainer(_state.CurrentOrganization)
                .HasPartitionKey(PartitionKey)
                .HasDiscriminator<string>("Type");

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
