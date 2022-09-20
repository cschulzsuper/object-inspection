namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectionAuditScheduler
{
    public void Schedule(BusinessObjectInspection inspection);
}