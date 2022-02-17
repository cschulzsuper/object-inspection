using System.ComponentModel.DataAnnotations;
using Super.Paula.Validation;

namespace Super.Paula.Application.Inventory.Events
{
    public class BusinessObjectInspectorEvent
    {
        [StringLength(140)]
        public string? BusinessObjectDisplayName { get; set; }

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string? NewInspector { get; set; }

        [KebabCase]
        [StringLength(140)]
        [UniqueName]
        public string? OldInspector { get; set; }
    }
}