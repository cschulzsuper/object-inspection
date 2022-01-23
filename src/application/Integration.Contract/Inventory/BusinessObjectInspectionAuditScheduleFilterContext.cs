namespace Super.Paula.Application.Inventory
{
    public record BusinessObjectInspectionAuditScheduleFilterContext(
        BusinessObjectInspection Inspection,
        (int Date, int Time) Limit);
}
