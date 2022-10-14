using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Administration;
using ChristianSchulz.ObjectInspection.Application.Guidelines;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Administration;

public class InspectorMapping : IEntityTypeConfiguration<Inspector>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public InspectorMapping(ObjectInspectionContextState state)
    {
        _state = state;
    }

    public void Configure(EntityTypeBuilder<Inspector> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<InspectorPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(Inspector.UniqueName));

        builder
            .ToTable(nameof(Inspector), _state.CurrentOrganization);

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
            .Property(x => x.Organization)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.OrganizationDisplayName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.OrganizationActivated)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Identity)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Activated)
            .IsRequired();

        var businessObjectsBuilder = builder
            .OwnsMany(x => x.BusinessObjects)
            .ToJson();

        businessObjectsBuilder
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        businessObjectsBuilder
            .Property(x => x.DisplayName)
            .HasMaxLength(140)
            .IsRequired();

        businessObjectsBuilder
            .Property(x => x.AuditSchedulePlannedAuditDate)
            .IsRequired();

        businessObjectsBuilder
            .Property(x => x.AuditSchedulePlannedAuditTime)
            .IsRequired();

        businessObjectsBuilder
            .Property(x => x.AuditScheduleDelayed)
            .IsRequired();

        businessObjectsBuilder
            .Property(x => x.AuditSchedulePending)
            .IsRequired();
    }
}