using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public interface IBusinessObjectInspectionEventService
    {
        ValueTask CreateBusinessObjectInspectionAuditScheduleEventAsync(ICollection<BusinessObjectInspection> inspections);
    }
}