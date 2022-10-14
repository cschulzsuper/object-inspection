using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Operation;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Operation;

public class ExtensionMapping : IEntityTypeConfiguration<Extension>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public ExtensionMapping(ObjectInspectionContextState state)
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
            .ToTable(nameof(Extension), _state.CurrentOrganization);

        builder
            .Property(x => x.Id)
            .HasValueGenerator<IdValueGenerator>()
            ;

        builder
            .Property(x => x.ETag)
            .IsRowVersion()
            .HasConversion(
                v => Convert.FromBase64String(v),
                v => Convert.ToBase64String(v));

        builder
            .Property(x => x.AggregateType)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .OwnsMany(x => x.Fields, fieldsBuilder =>
            {
                fieldsBuilder
                    .ToJson();

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