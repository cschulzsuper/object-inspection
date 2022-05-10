using System.Collections.Generic;

namespace Super.Paula.Client.Localization
{
    public interface ITranslationHandler
    {
        TranslationResponse Get(string hash);

        TranslationResponse Get(string category, string hash);

        TranslationResponse Create(TranslationRequest request);

        IEnumerable<TranslationResponse> GetAll(string query, int skip, int take);

        SearchTranslationResponse Search(string query);
    }
}
