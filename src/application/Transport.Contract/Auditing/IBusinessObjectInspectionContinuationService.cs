using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectionContinuationService
{
    ValueTask AddCreateBusinessObjectInspectionAuditRecordContinuationAsync(BusinessObjectInspection inspection);
}