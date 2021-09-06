using System.Collections.Immutable;

namespace Super.Paula.Web.Shared.Handling.Responses
{
    public class QueryAuthorizationsResponse
    {
        public ISet<string> Values { get; set; } = ImmutableHashSet.Create<string>();
    }
}