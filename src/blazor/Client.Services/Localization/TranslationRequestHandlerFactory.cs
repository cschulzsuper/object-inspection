using ChristianSchulz.ObjectInspection.Application.Localization;
using System.Globalization;

namespace ChristianSchulz.ObjectInspection.Client.Localization;

public class TranslationRequestHandlerFactory
{

    public static ITranslationRequestHandler Create()
    {
        var currentCulture = CultureInfo.CurrentCulture.Name;

        return currentCulture switch
        {
            "de" or "de-AT" or "de-CH" or "de-DE" or "de-LI" or "de-LU"
                => new LocalGermanTranslationRequestHandler(),

            _ => new LocalFallbackTranslationRequestHandler(),
        };
    }
}