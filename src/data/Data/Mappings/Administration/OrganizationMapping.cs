using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Administration;

namespace Super.Paula.Data.Mappings.Administration
{
    public class OrganizationMapping : IEntityTypeConfiguration<Organization>
    {
        public string PartitionKey = nameof(PartitionKey);

        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<OrganizationPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(Organization.UniqueName));

            builder
                .ToContainer("_administration")
                .HasPartitionKey(PartitionKey);

            builder
                .HasDiscriminator<string>("discriminator")
                .HasValue("organization");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

            builder
                .Property(x => x.UniqueName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.DisplayName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.ChiefInspector)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Activated)
                .IsRequired();
        }
    }
}
