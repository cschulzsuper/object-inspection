using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Application.Inventory.Responses
{
    public partial class BusinessObjectResponse
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;
        public string ETag { get; set; } = string.Empty;
    }
}