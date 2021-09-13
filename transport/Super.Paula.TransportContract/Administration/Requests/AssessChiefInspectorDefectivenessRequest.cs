using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Administration.Requests
{
    public class AssessChiefInspectorDefectivenessRequest
    {
        [Required]
        [StringLength(140)]
        public string Organization { get; set; } = string.Empty;
    }
}