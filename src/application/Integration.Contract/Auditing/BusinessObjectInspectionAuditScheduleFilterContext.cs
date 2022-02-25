namespace Super.Paula.Application.Auditing
{
    public record BusinessObjectInspectionAuditScheduleFilterContext(
        BusinessObjectInspection Inspection,
        (int Date, int Time) Limit);
}
