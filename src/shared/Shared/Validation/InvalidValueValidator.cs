using System.Linq;

namespace Super.Paula.Shared.Validation;

public static class InvalidValueValidator
{
    public static bool IsValid(object value, params object[] range)
        => !range.Contains(value);
}