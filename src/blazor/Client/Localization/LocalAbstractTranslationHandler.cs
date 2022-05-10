using Super.Paula.Application.Auth.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Client.Localization
{
    public abstract class LocalAbstractTranslationHandler : ITranslationHandler
    {
        private const string SearchTermKeyPlaceholder = "placeholder";
        private const string SearchTermKeyCategory = "category";

        public abstract IDictionary<(string? Category, string Hash), TranslationInfo> Translations { get; }

        public TranslationResponse Get(string hash)
        {
            var translationExists = Translations.TryGetValue((null, hash), out var translation);

            if(!translationExists)
            {
                throw new TranslationNotFoundException($"Translation for hash '{hash}' not found.");
            }

            return new TranslationResponse
            {
                Category = null,
                Hash = hash,
                Value = translation!.Value!
            };
        }

        public TranslationResponse Get(string category, string hash)
        {
            var translationExists = Translations.TryGetValue((category, hash), out var translation);

            if (!translationExists)
            {
                throw new TranslationNotFoundException($"Translation in catgeory '{category}' for hash '{hash}' not found.");
            }

            return new TranslationResponse
            {
                Category = category,
                Hash = hash,
                Value = translation!.Value!
            };
        }

        public TranslationResponse Create(TranslationRequest request)
        {
            Translations.Add((request.Category, request.Hash), new(request.Value, true));

            return new TranslationResponse
            {
                Category = request.Category,
                Hash = request.Hash,
                Value = request.Value
            };
        }

        public IEnumerable<TranslationResponse> GetAll(string query, int skip, int take)
            => WhereSearchQuery(Translations, query)
                .Skip(skip)
                .Take(take)
                .Select(entry => new TranslationResponse
                {
                    Category = entry.Key.Category,
                    Hash = entry.Key.Hash,
                    Value = entry.Value.Value
                });

        public SearchTranslationResponse Search(string query)
        {
            var enumerable = WhereSearchQuery(Translations, query);

            var topResult = enumerable.Take(50)
                .Select(entry => new TranslationResponse
                {
                    Category = entry.Key.Category,
                    Hash = entry.Key.Hash,
                    Value = entry.Value.Value
                })
                .ToHashSet();

            return new SearchTranslationResponse
            {
                TotalCount = enumerable.Count(),
                TopResults = topResult
            };
        }

        private static IEnumerable<KeyValuePair<(string? Category, string Hash), TranslationInfo>> WhereSearchQuery(
            IDictionary<(string? Category, string Hash), TranslationInfo> query, string searchQuery)
        {
            var searchTerms = SearchQueryParser.Parse(searchQuery);
            var placeholder = searchTerms.GetValidSearchTermValues<bool>(SearchTermKeyPlaceholder);
            var category = searchTerms.GetValidSearchTermValues<string>(SearchTermKeyCategory);

            var strin = string.Join(',', placeholder);

            var x = query
                .Where(x => !category.Any() || category.Contains(x.Key.Category))
                .Where(x => !placeholder.Any() || placeholder.Contains(x.Value.Placeholder))
                .OrderBy(x => x.Key)
                .ToList();

            return x;
        }
    }
}
