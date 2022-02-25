using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectEventService : IBusinessObjectEventService
    {
        private readonly ClaimsPrincipal _user;
        private readonly IEventStorage _eventStorage;

        public BusinessObjectEventService(
            ClaimsPrincipal user,
            IEventStorage eventStorage)
        {
            _user = user;
            _eventStorage = eventStorage;
        }

        public async ValueTask CreateBusinessObjectEventAsync(BusinessObject businessObject)
        {
            var @event = new BusinessObjectEvent(
                businessObject.UniqueName,
                businessObject.DisplayName);

            await _eventStorage.AddAsync(@event, _user);
        }

        public async ValueTask CreateBusinessObjectDeletionEventAsync(string businessObject)
        {
            var @event = new BusinessObjectDeletionEvent(businessObject);

            await _eventStorage.AddAsync(@event, _user);
        }

        public async ValueTask CreateBusinessObjectInspectorEventAsync(BusinessObject businessObject, string newInspector, string oldInspector)
        {
            var @event = new BusinessObjectInspectorEvent(
                businessObject.UniqueName,
                businessObject.DisplayName,
                newInspector,
                oldInspector);

            await _eventStorage.AddAsync(@event, _user);
        }

        public async ValueTask CreateBusinessObjectInspectionAuditEventAsync(BusinessObject businessObject, BusinessObjectInspection inspection)
        {
            if (inspection.AuditDate == default)
            {
                return;
            }

            var @event = new BusinessObjectInspectionAuditEvent(
                businessObject.UniqueName,
                businessObject.DisplayName,
                inspection.AuditInspector,
                inspection.UniqueName,
                inspection.DisplayName,
                inspection.AuditAnnotation,
                inspection.AuditResult,
                inspection.AuditDate,
                inspection.AuditTime);

            await _eventStorage.AddAsync(@event, _user);
        }

        public async ValueTask CreateBusinessObjectInspectionAuditScheduleEventAsync(BusinessObject businessObject)
        {
            var inspection = businessObject.Inspections
                .Where(x => x.AuditSchedule.Appointments.Any())
                .OrderBy(x => x.AuditSchedule.Appointments
                    .Min(y => (y.PlannedAuditDate, y.PlannedAuditTime).ToDateTime()))
                .FirstOrDefault();

            if (inspection == null)
            {
                return;
            }

            var @event = new BusinessObjectInspectionAuditScheduleEvent(
                businessObject.UniqueName,
                businessObject.Inspector,
                inspection.AuditSchedule.Appointments.First().PlannedAuditDate,
                inspection.AuditSchedule.Appointments.First().PlannedAuditTime,
                inspection.AuditSchedule.Threshold);

            await _eventStorage.AddAsync(@event, _user);
        }

    }
}