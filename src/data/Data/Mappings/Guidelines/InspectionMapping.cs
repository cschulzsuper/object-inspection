using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Guidelines;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Guidelines;

public class InspectionMapping : IEntityTypeConfiguration<Inspection>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public InspectionMapping(ObjectInspectionContextState state)
    {
        _state = state;
    }

    public void Configure(EntityTypeBuilder<Inspection> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<InspectionPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(Inspection.UniqueName));

        builder
            .ToContainer(_state.CurrentOrganization)
            .HasPartitionKey(PartitionKey);

        builder
            .HasDiscriminator<string>("discriminator")
            .HasValue("inspection");

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
            .Property(x => x.Text)
            .HasMaxLength(4000)
            .IsRequired();

        builder
            .Property(x => x.Activated)
            .IsRequired();
    }
}