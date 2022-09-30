using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ChristianSchulz.ObjectInspection.Client.Components.Forms;

public class InputDayNumber : InputBase<int>
{
    private readonly string _typeAttributeValue = "date";

    private readonly string _format = "yyyy-MM-dd";

    private readonly string _parsingErrorMessage = "The {{0}} field must be an int.";

    /// <summary>
    /// Gets or sets the error message used when displaying an a parsing error.
    /// </summary>
    [Parameter] public string ParsingErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the associated <see cref="ElementReference"/>.
    /// <para>
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </para>
    /// </summary>
    [DisallowNull] public ElementReference? Element { get; protected set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "input");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "type", _typeAttributeValue);
        builder.AddAttribute(3, "class", CssClass);
        builder.AddAttribute(4, "value", BindConverter.FormatValue(CurrentValueAsString));
        builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string?>(this, value => CurrentValueAsString = value, CurrentValueAsString));
        builder.AddElementReferenceCapture(6, inputReference => Element = inputReference);
        builder.CloseElement();
    }

    protected override string FormatValueAsString(int value)
    {
        var dateOnly = DateOnly.FromDayNumber(value);

        return BindConverter.FormatValue(dateOnly, _format, CultureInfo.InvariantCulture);
    }

    protected override bool TryParseValueFromString(string? value, out int result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out DateOnly dateOnly))
        {
            validationErrorMessage = null;
            result = dateOnly.DayNumber;
            return true;
        }

        validationErrorMessage = string.Format(CultureInfo.InvariantCulture, _parsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
        result = default;
        return false;
    }
}