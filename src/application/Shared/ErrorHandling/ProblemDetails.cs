using System.Collections.Generic;

namespace Super.Paula.ErrorHandling
{
    public class ProblemDetails
    {
        public string Type { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int? Status { get; set; }

        public string Detail { get; set; } = string.Empty;

        public string Instance { get; set; } = string.Empty;

        public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
    }
}