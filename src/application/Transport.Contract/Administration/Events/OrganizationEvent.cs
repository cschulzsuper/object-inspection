using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Events
{
    public class OrganizationEvent
    {
        [StringLength(140)]
        public string? DisplayName { get; set; }
        public bool? Activated { get; set; }
    }
}