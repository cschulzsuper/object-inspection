using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Guidelines.Events
{
    [AllowedSubscriber(AllowedSubscribers.InventoryBusinessObject)]
    [AllowedSubscriber(AllowedSubscribers.AuditingBusinessObjectInspectionAudit)]
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

}