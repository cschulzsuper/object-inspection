using System.Collections.Generic;
using System.Collections.Immutable;

namespace ChristianSchulz.ObjectInspection.Application.Localization.Responses;

public class SearchTranslationResponse
{
    public int TotalCount { get; set; }
    public ISet<TranslationResponse> TopResults { get; set; } = ImmutableHashSet.Create<TranslationResponse>();
}