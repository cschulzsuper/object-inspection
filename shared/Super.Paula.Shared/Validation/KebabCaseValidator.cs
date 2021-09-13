using System.Text.RegularExpressions;

namespace Super.Paula.Validation
{
    public static class KebabCaseValidator
    {
        public static bool IsValid(string value)
            => Regex.IsMatch((string)value, "^[a-z0-9-]*$");
    }
}
