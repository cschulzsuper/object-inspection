using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Guidelines;

namespace Super.Paula.Data.Mappings.Guidelines
{
    public class InspectionMapping : IEntityTypeConfiguration<Inspection>
    {
        public string PartitionKey = "Organization";

        public void Configure(EntityTypeBuilder<Inspection> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<InspectionPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(Inspection.UniqueName));

            builder
                .ToContainer(nameof(Inspection))
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
                .Property(x => x.Text)
                .HasMaxLength(4000)
                .IsRequired();

            builder
                .Property(x => x.Activated)
                .IsRequired();
        }
    }
}
