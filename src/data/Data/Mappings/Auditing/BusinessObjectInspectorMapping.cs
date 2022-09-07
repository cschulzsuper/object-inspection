using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Auditing;

namespace Super.Paula.Data.Mappings.Auditing;

public class BusinessObjectInspectorMapping : IEntityTypeConfiguration<BusinessObjectInspector>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly PaulaContextState _state;

    public BusinessObjectInspectorMapping(PaulaContextState state)
    {
        _state = state;
    }

    public void Configure(EntityTypeBuilder<BusinessObjectInspector> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<BusinessObjectInspectorPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(BusinessObjectInspector.Inspector));

        builder
            .ToContainer(_state.CurrentOrganization)
            .HasPartitionKey(PartitionKey);

        builder
            .HasDiscriminator<string>("discriminator")
            .HasValue("business-object-inspector");

        builder
            .Property(x => x.ETag)
            .IsETagConcurrency();

        builder
            .Property(x => x.Inspector)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.BusinessObject)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.BusinessObjectDisplayName)
            .HasMaxLength(140)
            .IsRequired();
    }
}