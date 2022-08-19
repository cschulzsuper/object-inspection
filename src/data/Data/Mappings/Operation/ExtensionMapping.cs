﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Operation;

namespace Super.Paula.Data.Mappings.Operation
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
                .HasKey(PartitionKey, nameof(Extension.AggregateType));

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
                .Property(x => x.AggregateType)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .OwnsMany(x => x.Fields, fieldsBuilder =>
                {
                    fieldsBuilder
                        .Property(x => x.UniqueName)
                        .HasMaxLength(140)
                        .IsRequired();

                    fieldsBuilder
                        .Property(x => x.DisplayName)
                        .HasMaxLength(140)
                        .IsRequired();

                    fieldsBuilder
                        .Property(x => x.DataType)
                        .HasMaxLength(140)
                        .IsRequired();

                    fieldsBuilder
                        .Property(x => x.DataName)
                        .HasMaxLength(140)
                        .IsRequired();
                });

        }
    }
}
