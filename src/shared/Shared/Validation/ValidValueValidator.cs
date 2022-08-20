using System.Linq;

namespace Super.Paula.Shared.Validation;

public static class ValidValueValidator
{
    public static bool IsValid(object value, params object[] range)
        => range.Contains(value);
}