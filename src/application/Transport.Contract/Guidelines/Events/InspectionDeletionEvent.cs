using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Guidelines.Events;

[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspectionAuditRecord)]
[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspection)]
public record InspectionDeletionEvent(

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName)

    : EventBase;