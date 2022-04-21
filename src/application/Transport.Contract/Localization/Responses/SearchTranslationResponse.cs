using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Paula.Client.Localization
{
    public class SearchTranslationResponse
    {
        public int TotalCount { get; set; }
        public ISet<TranslationResponse> TopResults { get; set; } = ImmutableHashSet.Create<TranslationResponse>();
    }
}