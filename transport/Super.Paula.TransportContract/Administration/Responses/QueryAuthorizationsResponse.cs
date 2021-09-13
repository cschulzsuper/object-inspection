using System.Collections.Immutable;

namespace Super.Paula.Administration.Responses
{
    public class QueryAuthorizationsResponse
    {
        public ISet<string> Values { get; set; } = ImmutableHashSet.Create<string>();
    }
}