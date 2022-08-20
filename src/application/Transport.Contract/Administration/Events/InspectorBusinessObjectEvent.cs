using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Administration.Events;

[AllowedSubscriber(AllowedSubscribers.Notification)]
public record InspectorBusinessObjectEvent
(
    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName,

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string BusinessObject,

    [StringLength(140)]
    string BusinessObjectDisplayName,

    bool NewAuditSchedulePending,

    bool OldAuditSchedulePending,

    bool NewAuditScheduleDelayed,

    bool OldAuditScheduleDelayed)

    : EventBase;