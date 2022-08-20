using Super.Paula.Application.Localization;
using Super.Paula.Application.Localization.Exceptions;
using Super.Paula.Application.Localization.Requests;
using Super.Paula.Application.Localization.Responses;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Client.Localization;

public class LocalFallbackTranslationRequestHandler : ITranslationRequestHandler
{
    public TranslationResponse Get(string hash)
        => throw new TranslationNotFoundException($"Translation for hash '{hash}' not found.");
    public TranslationResponse Get(string category, string hash)
        => throw new TranslationNotFoundException($"Translation in category '{category}' for hash '{hash}' not found.");

    public TranslationResponse Create(TranslationRequest request)
    {
        return new TranslationResponse
        {
            Category = request.Category,
            Hash = request.Hash,
            Value = request.Value
        };
    }

    public IEnumerable<TranslationResponse> GetAll(string query, int skip, int take)
        => Enumerable.Empty<TranslationResponse>();

    public SearchTranslationResponse Search(string query)
        => new();
}