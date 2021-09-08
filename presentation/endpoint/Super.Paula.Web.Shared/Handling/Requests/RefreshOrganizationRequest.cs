using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class RefreshOrganizationRequest
    {
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

        public bool Activated { get; set; }
    }
}