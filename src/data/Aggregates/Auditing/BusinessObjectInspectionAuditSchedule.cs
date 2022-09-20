using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public class BusinessObjectInspectionAuditSchedule
{
    public ISet<BusinessObjectInspectionAuditScheduleExpression> Expressions { get; set; } = new HashSet<BusinessObjectInspectionAuditScheduleExpression>();

    public int Threshold { get; set; }

    public ISet<BusinessObjectInspectionAuditScheduleTimestamp> Additionals { get; set; } = new HashSet<BusinessObjectInspectionAuditScheduleTimestamp>();

    public ISet<BusinessObjectInspectionAuditScheduleTimestamp> Omissions { get; set; } = new HashSet<BusinessObjectInspectionAuditScheduleTimestamp>();

    public ISet<BusinessObjectInspectionAuditScheduleTimestamp> Appointments { get; set; } = new HashSet<BusinessObjectInspectionAuditScheduleTimestamp>();
}