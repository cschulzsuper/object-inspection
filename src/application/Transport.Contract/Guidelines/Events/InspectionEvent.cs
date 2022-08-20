using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Guidelines.Events;

[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspection)]
[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspectionAuditRecord)]
public record InspectionEvent(

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName,

    [StringLength(140)]
    string DisplayName,

    [StringLength(4000)]
    string Text,

    bool Activated)

    : EventBase;