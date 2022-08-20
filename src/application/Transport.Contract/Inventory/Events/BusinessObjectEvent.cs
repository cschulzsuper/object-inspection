using Super.Paula.Shared.Orchestration;
using System.ComponentModel.DataAnnotations;
using Super.Paula.Shared.Validation;

namespace Super.Paula.Application.Inventory.Events;

[AllowedSubscriber(AllowedSubscribers.BusinessObjectInspectionAuditRecord)]
public record BusinessObjectEvent(

    [StringLength(140)]
    [UniqueName]
    [KebabCase]
    string UniqueName,

    [StringLength(140)]
    string DisplayName)

    : EventBase;