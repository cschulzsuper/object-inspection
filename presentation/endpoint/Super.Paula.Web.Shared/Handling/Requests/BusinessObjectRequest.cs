using System.Collections.Immutable;

namespace Super.Paula.Web.Shared.Handling.Requests
{
    public class BusinessObjectRequest
    {
        public string Inspector { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string UniqueName { get; set; } = string.Empty;
    }
}