using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class RefreshBusinessObjectRequest
    {
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;
    }
}