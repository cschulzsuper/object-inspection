using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Administration;

namespace Super.Paula.Data.Mappings.Administration
{
    public class InspectorMapping : IEntityTypeConfiguration<Inspector>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;

        public InspectorMapping(PaulaContextState state)
        {
            _state = state;
        }

        public void Configure(EntityTypeBuilder<Inspector> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<InspectorPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(Inspector.UniqueName));

            builder
                .ToContainer(_state.CurrentOrganization)
                .HasPartitionKey(PartitionKey);

            builder
                .HasDiscriminator<string>("discriminator")
                .HasValue("inspector");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

            builder
                .Property(x => x.Organization)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.OrganizationDisplayName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.OrganizationActivated)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.UniqueName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Identity)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Activated)
                .IsRequired();

            var businessObjectsBuilder = builder
                .OwnsMany(x => x.BusinessObjects);

            businessObjectsBuilder
                .Property(x => x.UniqueName)
                .HasMaxLength(140)
                .IsRequired();

            businessObjectsBuilder
                .Property(x => x.DisplayName)
                .HasMaxLength(140)
                .IsRequired();

            businessObjectsBuilder
                .Property(x => x.AuditSchedulePlannedAuditDate)
                .IsRequired();

            businessObjectsBuilder
                .Property(x => x.AuditSchedulePlannedAuditTime)
                .IsRequired();

            businessObjectsBuilder
                .Property(x => x.AuditScheduleDelayed)
                .IsRequired();

            businessObjectsBuilder
                .Property(x => x.AuditSchedulePending)
                .IsRequired();
        }
    }
}
