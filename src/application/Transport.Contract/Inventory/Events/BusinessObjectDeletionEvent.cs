using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Inventory.Events;

[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspectionAuditRecord)]
[AllowedSubscriber(AllowedSubscribers.Inspector)]
public record BusinessObjectDeletionEvent(

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName)

    : EventBase;