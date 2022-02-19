using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    [AllowedSubscriber(AllowedSubscribers.AdministrationInspector)]
    public record BusinessObjectInspectionAuditScheduleEvent(

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string UniqueName,

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string Inspector,

        [DayNumber]
        int PlannedAuditDate,

        [Milliseconds]
        int PlannedAuditTime,

        [Milliseconds]
        int Threshold)

        : EventBase;
}