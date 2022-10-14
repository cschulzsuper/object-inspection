using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using System.Collections.Concurrent;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;

public class BusinessObjectInspectionAuditRecordMapping : IEntityTypeConfiguration<BusinessObjectInspectionAuditRecord>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public BusinessObjectInspectionAuditRecordMapping(ObjectInspectionContextState state)
    {
        _state = state;
    }

    public void Configure(EntityTypeBuilder<BusinessObjectInspectionAuditRecord> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<BusinessObjectInspectionAuditRecordPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey,
                nameof(BusinessObjectInspectionAuditRecord.BusinessObject),
                nameof(BusinessObjectInspectionAuditRecord.Inspection),
                nameof(BusinessObjectInspectionAuditRecord.AuditTime));

        builder
            .ToTable(nameof(BusinessObjectInspectionAuditRecord), _state.CurrentOrganization);

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
            .Property(x => x.BusinessObject)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.BusinessObjectDisplayName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Inspection)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.InspectionDisplayName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Inspector)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Result)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.AuditDate)
            .IsRequired();

        builder
            .Property(x => x.AuditTime)
            .IsRequired();

        builder
            .Property(x => x.Annotation)
            .HasMaxLength(4000)
            .IsRequired();

    }
}