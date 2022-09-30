using ChristianSchulz.ObjectInspection.Application.Auditing.Events;
using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChristianSchulz.ObjectInspection.Shared;

namespace ChristianSchulz.ObjectInspection.Application.Auditing;

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

    public async ValueTask CreateBusinessObjectInspectionAuditScheduleEventAsync(ICollection<BusinessObjectInspection> inspections)
    {
        var inspection = inspections
            .Where(x => x.AuditSchedule.Appointments.Any())
            .MinBy(x => x.AuditSchedule.Appointments
                .Min(y =>
                    new DateTimeNumbers(
                        y.PlannedAuditDate,
                        y.PlannedAuditTime)
                    .ToGlobalDateTime()));

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