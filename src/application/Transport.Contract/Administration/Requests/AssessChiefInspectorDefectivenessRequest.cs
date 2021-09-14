using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Application.Administration.Requests
{
    public class AssessChiefInspectorDefectivenessRequest
    {
        [Required]
        [StringLength(140)]
        public string Organization { get; set; } = string.Empty;
    }
}