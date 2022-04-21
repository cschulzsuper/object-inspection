using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Super.Paula.Client.Localization
{
    public interface ITranslator<T>
    {
        string this[FormattableString value] { get; }

        MarkupString Markdown(FormattableString value);
    }
}
