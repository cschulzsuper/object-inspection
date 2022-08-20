namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionAuditScheduler
{
    public void Schedule(BusinessObjectInspection inspection);
}