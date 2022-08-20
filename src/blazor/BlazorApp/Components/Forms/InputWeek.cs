using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Super.Paula.Shared;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Super.Paula.Client.Components.Forms;

public class InputWeek : InputBase<CalenderWeek>
{
    private readonly string _typeAttributeValue = "week";

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
        builder.AddAttribute(6, "onkeydown", "return false;");
        builder.AddElementReferenceCapture(6, inputReference => Element = inputReference);
        builder.CloseElement();
    }

    protected override string FormatValueAsString(CalenderWeek value)
        => $"{value.Year}-W{value.Week:00}";

    protected override bool TryParseValueFromString(string? value, out CalenderWeek result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (value == null)
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, _parsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
            result = CalenderWeek.Zero;
            return false;
        }

        var calenderWeek = value.Split("-W");
        if (calenderWeek.Length != 2)
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, _parsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
            result = CalenderWeek.Zero;
            return false;
        }

        var yearIsValid = int.TryParse(calenderWeek[0], out var year);
        var weekIsValid = int.TryParse(calenderWeek[1], out var week);

        if (!yearIsValid || !weekIsValid)
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, _parsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
            result = CalenderWeek.Zero;
            return false;
        }

        validationErrorMessage = string.Format(CultureInfo.InvariantCulture, _parsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
        result = new CalenderWeek(year, week);
        return true;
    }
}