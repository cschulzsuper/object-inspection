using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Operation;
using ChristianSchulz.ObjectInspection.Application.Inventory;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

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
            .ToTable(nameof(DistinctionType), _state.CurrentOrganization);

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
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .OwnsMany(x => x.Fields, fieldsBuilder =>
            {
                fieldsBuilder
                    .ToJson();

                fieldsBuilder
                    .Property(x => x.ExtensionField)
                    .HasMaxLength(140)
                    .IsRequired();
            });

    }
}