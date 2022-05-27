using Super.Paula.Application.Orchestration;
using Super.Paula.Validation;
using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    [AllowedSubscriber(AllowedSubscribers.Inspector)]
    [AllowedSubscriber(AllowedSubscribers.Notification)]
    public record BusinessObjectInspectorEvent(

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string UnqiueName,

        [StringLength(140)]
        string DisplayName,

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string? NewInspector,

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        string OldInspector)

        : EventBase();
}