using ChristianSchulz.ObjectInspection.Application.Localization;
using ChristianSchulz.ObjectInspection.Application.Localization.Exceptions;
using ChristianSchulz.ObjectInspection.Application.Localization.Requests;
using ChristianSchulz.ObjectInspection.Application.Localization.Responses;
using System.Collections.Generic;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Client.Localization;

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