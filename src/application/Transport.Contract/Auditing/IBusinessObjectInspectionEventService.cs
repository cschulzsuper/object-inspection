using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionEventService
    {
        ValueTask CreateBusinessObjectInspectionAuditEventAsync(BusinessObjectInspection inspection);
        ValueTask CreateBusinessObjectInspectionAuditScheduleEventAsync(ICollection<BusinessObjectInspection> inspections);
    }
}