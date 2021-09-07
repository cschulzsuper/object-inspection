using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Aggregates.Inventory;
using Super.Paula.Data.Mapping.PartitionKeyValueGenerators;

namespace Super.Paula.Data.Mapping
{
    public class BusinessObjectMapping : IEntityTypeConfiguration<BusinessObject>
    {
        public string PartitionKey = $"Organization";

        public void Configure(EntityTypeBuilder<BusinessObject> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<BusinessObjectPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(BusinessObject.UniqueName));

            builder
                .ToContainer(nameof(BusinessObject))
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
                .Property(x => x.ActivatedGlobally)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.DisplayName)
                .IsRequired();

            inspectionsBuilder
                .Property(x => x.Text)
                .HasMaxLength(4000)
                .IsRequired();
        }
    }
}
