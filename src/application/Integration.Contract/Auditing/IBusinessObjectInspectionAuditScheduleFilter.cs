namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionAuditScheduleFilter
    {
        public void Apply(BusinessObjectInspectionAuditScheduleFilterContext context);
    }
}
