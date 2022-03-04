using Microsoft.AspNetCore.Components;
using System;

namespace Super.Paula.Client.Localization
{
    public interface ITranslator
    {
        string this[FormattableString value] { get; }

        MarkupString Markdown(FormattableString value);
    }
}
