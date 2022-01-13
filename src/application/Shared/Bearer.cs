using System.Text.Json.Serialization;

namespace Super.Paula
{
    public class Bearer
    {
        public string? Organization { get; set; }
        public string? Inspector { get; set; }
        public string? Proof { get; set; }

        public string? ImpersonatorOrganization { get; set; }
        public string? ImpersonatorInspector { get; set; }
    }
}
