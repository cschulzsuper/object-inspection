using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Auditing;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Auditing;

public class BusinessObjectInspectionMapping : IEntityTypeConfiguration<BusinessObjectInspection>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public BusinessObjectInspectionMapping(ObjectInspectionContextState state)
    {
        _state = state;
    }

    public void Configure(EntityTypeBuilder<BusinessObjectInspection> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<BusinessObjectInspectionPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(BusinessObjectInspection.Inspection));

        builder
            .ToTable(nameof(BusinessObjectInspection), _state.CurrentOrganization);

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
            .Property(x => x.Activated)
            .IsRequired();

        builder
            .Property(x => x.AssignmentDate)
            .IsRequired();

        builder
            .Property(x => x.AssignmentTime)
            .IsRequired();

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
            .Property(x => x.InspectionText)
            .HasMaxLength(4000)
            .IsRequired();

        var auditBuilder = builder
            .OwnsOne(x => x.Audit)
            .ToJson();

        auditBuilder
            .Property(x => x.Annotation)
            .HasMaxLength(4000)
            .IsRequired();

        auditBuilder
            .Property(x => x.AuditDate)
            .IsRequired();

        auditBuilder
            .Property(x => x.AuditTime)
            .IsRequired();

        auditBuilder
            .Property(x => x.Inspector)
            .HasMaxLength(140)
            .IsRequired();

        auditBuilder
            .Property(x => x.Result)
            .HasMaxLength(140)
            .IsRequired();

        var auditScheduleBuilder = builder
            .OwnsOne(x => x.AuditSchedule)
            .ToJson();

        auditScheduleBuilder
            .Property(x => x.Threshold)
            .IsRequired();

        var auditScheduleExpressionsBuilder = auditScheduleBuilder
            .OwnsMany(x => x.Expressions)
            .ToJson();

        auditScheduleExpressionsBuilder
            .Property(x => x.CronExpression)
            .HasMaxLength(140)
            .IsRequired();

        var auditScheduleOmissionsBuilder = auditScheduleBuilder
            .OwnsMany(x => x.Omissions);

        auditScheduleOmissionsBuilder
            .Property(x => x.PlannedAuditDate)
            .IsRequired();

        auditScheduleOmissionsBuilder
            .Property(x => x.PlannedAuditTime)
            .IsRequired();

        var auditScheduleAdditionalsBuilder = auditScheduleBuilder
            .OwnsMany(x => x.Additionals);

        auditScheduleAdditionalsBuilder
            .Property(x => x.PlannedAuditDate)
            .IsRequired();

        auditScheduleAdditionalsBuilder
            .Property(x => x.PlannedAuditTime)
            .IsRequired();

        var auditScheduleAppointmentsBuilder = auditScheduleBuilder
            .OwnsMany(x => x.Appointments);

        auditScheduleAppointmentsBuilder
            .Property(x => x.PlannedAuditDate)
            .IsRequired();

        auditScheduleAppointmentsBuilder
            .Property(x => x.PlannedAuditTime)
            .IsRequired();
    }
}