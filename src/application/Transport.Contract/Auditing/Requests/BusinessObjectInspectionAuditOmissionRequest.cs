using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Requests;

public class BusinessObjectInspectionAuditOmissionRequest
{
    [DayNumber]
    public int PlannedAuditDate { get; set; }

    [Milliseconds]
    public int PlannedAuditTime { get; set; }
}