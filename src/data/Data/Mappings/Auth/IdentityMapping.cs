using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Auth;

namespace Super.Paula.Data.Mappings.Auth
{
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
                .ToContainer(nameof(Identity))
                .HasPartitionKey(PartitionKey)
                .HasNoDiscriminator();

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
}
