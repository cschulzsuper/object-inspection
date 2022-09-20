using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Auditing.Events;

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