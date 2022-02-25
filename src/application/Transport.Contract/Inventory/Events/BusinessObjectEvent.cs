using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    [AllowedSubscriber(AllowedSubscribers.AuditingBusinessObjectInspectionAuditRecord)]
    public record BusinessObjectEvent(

        [StringLength(140)]
        [UniqueName]
        [KebabCase]
        string UniqueName,

        [StringLength(140)]
        string DisplayName)

        : EventBase;
}