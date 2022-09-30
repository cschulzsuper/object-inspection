using ChristianSchulz.ObjectInspection.Application.Localization.Requests;
using ChristianSchulz.ObjectInspection.Application.Localization.Responses;
using System.Collections.Generic;

namespace ChristianSchulz.ObjectInspection.Application.Localization;

public interface ITranslationRequestHandler
{
    TranslationResponse Get(string hash);

    TranslationResponse Get(string category, string hash);

    TranslationResponse Create(TranslationRequest request);

    IEnumerable<TranslationResponse> GetAll(string query, int skip, int take);

    SearchTranslationResponse Search(string query);
}