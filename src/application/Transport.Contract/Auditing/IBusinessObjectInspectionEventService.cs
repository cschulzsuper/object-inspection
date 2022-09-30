using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

public interface IBusinessObjectInspectionEventService
{
    ValueTask CreateBusinessObjectInspectionAuditScheduleEventAsync(ICollection<BusinessObjectInspection> inspections);
}