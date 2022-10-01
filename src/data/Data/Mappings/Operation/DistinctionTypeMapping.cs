using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Operation;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Operation;

public class DistinctionTypeMapping : IEntityTypeConfiguration<DistinctionType>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public DistinctionTypeMapping(ObjectInspectionContextState state)
    {
        _state = state;
    }

    public void Configure(EntityTypeBuilder<DistinctionType> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<DistinctionTypePartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(DistinctionType.UniqueName));

        builder
            .ToContainer(_state.CurrentOrganization)
            .HasPartitionKey(PartitionKey);

        builder
            .HasDiscriminator<string>("discriminator")
            .HasValue("distinction-type");

        builder
            .Property(x => x.ETag)
            .IsETagConcurrency();

        builder
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .OwnsMany(x => x.Fields, fieldsBuilder =>
            {
                fieldsBuilder
                    .Property(x => x.ExtensionField)
                    .HasMaxLength(140)
                    .IsRequired();
            });

    }
}