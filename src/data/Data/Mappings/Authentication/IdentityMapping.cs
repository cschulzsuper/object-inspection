using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Authentication;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Authentication;

public class IdentityMapping : IEntityTypeConfiguration<Identity>
{
    public string PartitionKey = nameof(PartitionKey);

    public void Configure(EntityTypeBuilder<Identity> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<IdentityPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(Identity.UniqueName));

        builder
            .ToContainer("_auth")
            .HasPartitionKey(PartitionKey);

        builder
            .HasDiscriminator<string>("discriminator")
            .HasValue("identity");

        builder
            .Property(x => x.ETag)
            .IsETagConcurrency();

        builder
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.MailAddress)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Secret)
            .HasMaxLength(140)
            .IsRequired();
    }
}