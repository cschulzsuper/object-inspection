using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Guidelines;
using ChristianSchulz.ObjectInspection.Application.Communication;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

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
            .ToTable(nameof(Inspection), _state.CurrentOrganization);

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