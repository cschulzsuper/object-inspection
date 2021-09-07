using System.Collections.Immutable;

namespace Super.Paula.Web.Shared.Handling.Responses
{
    public partial class BusinessObjectResponse
    {
        public string UniqueName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Inspector { get; set; } = string.Empty;
        public ISet<BusinessObjectResponse.EmbeddedInspection> Inspections { get; set; } = ImmutableHashSet.Create<BusinessObjectResponse.EmbeddedInspection>();
    }
}