using System;

namespace Super.Paula.Application.Localization.Exceptions
{
    public class TranslationNotFoundException : Exception
    {
        public TranslationNotFoundException(FormattableString message)
            : base(message.ToString())
        {

        }

        public TranslationNotFoundException(FormattableString message, Exception innerException)
            : base(message.ToString(), innerException)
        {

        }
    }
}
