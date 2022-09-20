using ChristianSchulz.ObjectInspection.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using ChristianSchulz.ObjectInspection.Shared.Validation;

namespace ChristianSchulz.ObjectInspection.Application.Inventory.Events;

[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspectionAuditRecord)]
[AllowedSubscriber(AllowedSubscribers.Inspector)]
public record BusinessObjectDeletionEvent(

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName)

    : EventBase;