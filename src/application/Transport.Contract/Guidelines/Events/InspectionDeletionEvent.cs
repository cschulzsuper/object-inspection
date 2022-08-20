using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Guidelines.Events;

[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspectionAuditRecord)]
[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspection)]
public record InspectionDeletionEvent(

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName)

    : EventBase;