using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Administration;

namespace Super.Paula.Data.Mappings.Administration;

public class IdentityInspectorMapping : IEntityTypeConfiguration<IdentityInspector>
{
    public string PartitionKey = nameof(PartitionKey);

    public void Configure(EntityTypeBuilder<IdentityInspector> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<IdentityInspectorPartitionKeyValueGenerator>();

        builder
             .HasKey(PartitionKey,
                nameof(IdentityInspector.Organization),
                nameof(IdentityInspector.Inspector));

        builder
            .ToContainer("_administration")
            .HasPartitionKey(PartitionKey);

        builder
            .HasDiscriminator<string>("discriminator")
            .HasValue("identity-inspector");

        builder
            .Property(x => x.ETag)
            .IsETagConcurrency();

        builder
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Organization)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Inspector)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Activated)
            .IsRequired();
    }
}