using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Guidelines.Events;

[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspection)]
[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspectionAuditRecord)]
public record InspectionUpdateEvent(

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