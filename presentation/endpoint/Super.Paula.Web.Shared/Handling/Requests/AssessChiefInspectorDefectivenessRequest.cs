using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class AssessChiefInspectorDefectivenessRequest
    {
        [StringLength(140)]
        public string Organization { get; set; } = string.Empty;
    }
}