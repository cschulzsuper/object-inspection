using System;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

public static class InvalidStringValidator
{
    public static bool IsValid(string value, params string[] range)
        => !range.Contains(value, StringComparer.InvariantCultureIgnoreCase);
}