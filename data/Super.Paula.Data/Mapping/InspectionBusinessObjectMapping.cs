using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Aggregates.InspectionBusinessObjects;
using Super.Paula.Data.Mapping.PartitionKeyValueGenerators;

namespace Super.Paula.Data.Mapping
{
    public class InspectionBusinessObjectMapping : IEntityTypeConfiguration<InspectionBusinessObject>
    {
        public string PartitionKey = nameof(PartitionKey);

        public void Configure(EntityTypeBuilder<InspectionBusinessObject> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<InspectionBusinessObjectPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(InspectionBusinessObject.Inspection));

            builder
                .ToContainer(nameof(InspectionBusinessObject))
                .HasPartitionKey(PartitionKey)
                .HasNoDiscriminator();

            builder
                .Property(x => x.BusinessObject)
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
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.InspectionActivated)
                .IsRequired();

        }
    }
}
