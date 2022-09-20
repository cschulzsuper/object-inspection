using Microsoft.AspNetCore.Components;
using ChristianSchulz.ObjectInspection.Application.Localization;
using ChristianSchulz.ObjectInspection.Application.Localization.Exceptions;
using ChristianSchulz.ObjectInspection.Application.Localization.Requests;
using ChristianSchulz.ObjectInspection.Shared.Environment;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ChristianSchulz.ObjectInspection.Client.Localization;

public class Translator<T> : ITranslator<T>
{
    private readonly ITranslationRequestHandler _translationHandler;
    private readonly TranslationCategoryProvider _translationCategoryProvider;
    private readonly AppEnvironment _appEnvironment;

    public Translator(
        ITranslationRequestHandler translationHandler,
        TranslationCategoryProvider translationCategoryProvider,
        AppEnvironment appEnvironment)
    {
        _translationHandler = translationHandler;
        _translationCategoryProvider = translationCategoryProvider;
        _appEnvironment = appEnvironment;
    }

    public string this[FormattableString value]
        => this[value.Format, value.GetArguments()];

    public string this[string format, params object?[] arguments]
    {
        get
        {
            var valueBytes = Encoding.UTF8.GetBytes(format);
            var hashBytes = SHA1.HashData(valueBytes);

            var translationCategory = _translationCategoryProvider.Get<T>();
            var hashString = Convert.ToHexString(hashBytes).ToLower();

            try
            {
                var translation = translationCategory != null
                    ? _translationHandler.Get(translationCategory, hashString)
                    : _translationHandler.Get(hashString);

                return string.Format(translation.Value, arguments);
            }
            catch (TranslationNotFoundException)
            {
                if (_appEnvironment.IsDevelopment)
                {
                    _translationHandler.Create(new TranslationRequest
                    {
                        Category = translationCategory,
                        Hash = hashString,
                        Value = format
                    });
                }
                return arguments.Length > 0
                    ? string.Format(format, arguments)
                    : format;
            }
        }
    }

    public MarkupString Markdown(FormattableString value)
    {
        return new MarkupString(
            TrimParagraph(
                    Markdig.Markdown.ToHtml(value.ToString())));
    }

    private static string TrimParagraph(string html)
    {
        html = html.Trim();

        if (html.StartsWith("<p>"))
            html = html.Remove(0, 3);

        if (html.EndsWith("</p>"))
            html = html.Substring(0, html.LastIndexOf("</p>"));

        return html;
    }
}