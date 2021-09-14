using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Administration;
using Super.Paula.Data.Mapping.PartitionKeyValueGenerators;

namespace Super.Paula.Data.Mapping
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
                .ToContainer(nameof(Organization))
                .HasPartitionKey(PartitionKey)
                .HasNoDiscriminator();

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
