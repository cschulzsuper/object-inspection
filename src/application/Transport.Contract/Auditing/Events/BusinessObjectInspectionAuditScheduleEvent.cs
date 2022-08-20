using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Auditing.Events;

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

    : EventBase;