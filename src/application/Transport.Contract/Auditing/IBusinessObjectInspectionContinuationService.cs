using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing;

public interface IBusinessObjectInspectionContinuationService
{
    ValueTask AddCreateBusinessObjectInspectionAuditRecordContinuationAsync(BusinessObjectInspection inspection);
}