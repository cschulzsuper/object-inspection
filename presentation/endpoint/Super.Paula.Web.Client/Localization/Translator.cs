using Super.Paula.Web.Shared.Localization;

namespace Super.Paula.Web.Client.Localization
{
    public class Translator : ITranslator
    {
        public string this[string value] => value;
    }
}
