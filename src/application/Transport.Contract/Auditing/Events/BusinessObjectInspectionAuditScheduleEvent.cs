using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Auditing.Events
{
    [AllowedSubscriber(AllowedSubscribers.Inspector)]
    public record BusinessObjectInspectionAuditScheduleEvent(

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string BusinessObject,

        [DayNumber]
        int PlannedAuditDate,

        [Milliseconds]
        int PlannedAuditTime,

        [Milliseconds]
        int Threshold)

        : EventBase("event-business-object-inspection-audit-schedule");
}