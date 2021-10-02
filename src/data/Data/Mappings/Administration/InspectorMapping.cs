using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Administration;

namespace Super.Paula.Data.Mappings.Administration
{
    public class InspectorMapping : IEntityTypeConfiguration<Inspector>
    {
        public string PartitionKey = $"Organization";

        public void Configure(EntityTypeBuilder<Inspector> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<InspectorPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(Inspector.UniqueName));

            builder
                .ToContainer(nameof(Inspector))
                .HasPartitionKey(x => x.Organization)
                .HasNoDiscriminator();

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
                .Property(x => x.MailAddress)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Proof)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Secret)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Activated)
                .IsRequired();

        }
    }
}
