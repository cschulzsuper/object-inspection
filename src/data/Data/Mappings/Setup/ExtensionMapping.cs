using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Setup;

namespace Super.Paula.Data.Mappings.Setup
{
    public class ExtensionMapping : IEntityTypeConfiguration<Extension>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;

        public ExtensionMapping(PaulaContextState state)
        {
            _state = state;
        }

        public void Configure(EntityTypeBuilder<Extension> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<ExtensionPartitionKeyValueGenerator>();

            builder
                .HasKey(PartitionKey, nameof(Extension.Type));

            builder
                .ToContainer(_state.CurrentOrganization)
                .HasPartitionKey(PartitionKey);

            builder
                .HasDiscriminator<string>("discriminator")
                .HasValue("extension");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

            builder
                .Property(x => x.Type)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .OwnsMany(x => x.Fields, fieldsBuilder =>
                {
                    fieldsBuilder
                        .Property(x => x.Name)
                        .HasMaxLength(140)
                        .IsRequired();

                    fieldsBuilder
                        .Property(x => x.Type)
                        .HasMaxLength(140)
                        .IsRequired();
                });

        }
    }
}
