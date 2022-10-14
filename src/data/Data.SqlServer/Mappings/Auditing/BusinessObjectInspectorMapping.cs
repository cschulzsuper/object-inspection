using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;

public class BusinessObjectInspectorMapping : IEntityTypeConfiguration<BusinessObjectInspector>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public BusinessObjectInspectorMapping(ObjectInspectionContextState state)
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
            .ToTable(nameof(BusinessObjectInspector), _state.CurrentOrganization);

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