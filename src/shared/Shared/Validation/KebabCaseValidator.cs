﻿using System.Text.RegularExpressions;

namespace Super.Paula.Shared.Validation;

public static partial class KebabCaseValidator
{
    public static bool IsValid(object value)
        => value is string regex && KebabCaseRegex().IsMatch(regex);

    [GeneratedRegex("^[a-z0-9-]*$")]
    private static partial Regex KebabCaseRegex();
}