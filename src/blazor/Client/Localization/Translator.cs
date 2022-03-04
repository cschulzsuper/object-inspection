using Microsoft.AspNetCore.Components;
using System;

namespace Super.Paula.Client.Localization
{
    public class Translator : ITranslator
    {
        public string this[FormattableString value]
            => value.ToString();

        public MarkupString Markdown(FormattableString value)
        {
            return new MarkupString(
                TrimParagraph(
                        Markdig.Markdown.ToHtml(value.ToString())));
        }
    
        private string TrimParagraph(string html)
        {
            html = html.Trim();

            if (html.StartsWith("<p>"))
                html = html.Remove(0, 3);

            if (html.EndsWith("</p>"))
                html = html.Substring(0, html.LastIndexOf("</p>"));

            return html;
        }
    }
}
