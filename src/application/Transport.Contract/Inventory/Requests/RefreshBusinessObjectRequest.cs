using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Inventory.Requests
{
    public class RefreshBusinessObjectRequest
    {
        [Required]
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;
    }
}