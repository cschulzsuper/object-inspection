using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class RefreshInspectionRequest
    {
        [StringLength(140)]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(4000)]
        public string Text { get; set; } = string.Empty;
        public bool Activated { get; set; }
    }
}