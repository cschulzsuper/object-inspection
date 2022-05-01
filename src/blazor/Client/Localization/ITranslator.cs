using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Super.Paula.Client.Localization
{
    public interface ITranslator<T>
    {
        string this[FormattableString value] { get; }

        string this[string format, params object?[] arguments] { get; }

        MarkupString Markdown(FormattableString value);
    }
}
