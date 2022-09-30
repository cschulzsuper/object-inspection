using System.Text.RegularExpressions;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

public static partial class LowerCaseValidator
{
    public static bool IsValid(object value)
        => value is string regex && LowerCaseRegex().IsMatch(regex);

    [GeneratedRegex("^[a-z0-9]*$")]
    private static partial Regex LowerCaseRegex();
}