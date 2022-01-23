using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Events
{
    public class BusinessObjectEvent
    {
        [StringLength(140)]
        public string? DisplayName { get; set; }
    }
}