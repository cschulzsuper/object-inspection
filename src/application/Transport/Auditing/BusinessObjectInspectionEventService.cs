using Super.Paula;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Auditing
{
    public class BusinessObjectInspectionEventService : IBusinessObjectInspectionEventService
    {
        private readonly ClaimsPrincipal _user;
        private readonly IEventStorage _eventStorage;

        public BusinessObjectInspectionEventService(
            ClaimsPrincipal user,
            IEventStorage eventStorage)
        {
            _user = user;
            _eventStorage = eventStorage;
        }

        public async ValueTask CreateBusinessObjectInspectionAuditEventAsync(BusinessObjectInspection inspection)
        {
            if (inspection.Audit.AuditDate == default)
            {
                return;
            }

            var @event = new BusinessObjectInspectionAuditEvent(
                inspection.BusinessObject,
                inspection.BusinessObjectDisplayName,
                inspection.Audit.Inspector,
                inspection.Inspection,
                inspection.InspectionDisplayName,
                inspection.Audit.Annotation,
                inspection.Audit.Result,
                inspection.Audit.AuditDate,
                inspection.Audit.AuditTime);

            await _eventStorage.AddAsync(@event, _user);
        }

        public async ValueTask CreateBusinessObjectInspectionAuditScheduleEventAsync(ICollection<BusinessObjectInspection> inspections)
        {
            var inspection = inspections
                .Where(x => x.AuditSchedule.Appointments.Any())
                .OrderBy(x => x.AuditSchedule.Appointments
                    .Min(y => (y.PlannedAuditDate, y.PlannedAuditTime).ToDateTime()))
                .FirstOrDefault();

            if (inspection == null)
            {
                return;
            }

            var @event = new BusinessObjectInspectionAuditScheduleEvent(
                inspection.BusinessObject,
                inspection.AuditSchedule.Appointments.First().PlannedAuditDate,
                inspection.AuditSchedule.Appointments.First().PlannedAuditTime,
                inspection.AuditSchedule.Threshold);

            await _eventStorage.AddAsync(@event, _user);
        }
    }
}