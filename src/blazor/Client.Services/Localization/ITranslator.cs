using Microsoft.AspNetCore.Components;
using System;

namespace ChristianSchulz.ObjectInspection.Client.Localization;

public interface ITranslator<T>
{
    string this[FormattableString value] { get; }

    string this[string format, params object?[] arguments] { get; }

    MarkupString Markdown(FormattableString value);
}