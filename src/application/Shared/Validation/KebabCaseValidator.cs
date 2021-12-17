using System.Text.RegularExpressions;

namespace Super.Paula.Validation
{
    public static class KebabCaseValidator
    {
        public static bool IsValid(object value)
            => value is string regex && Regex.IsMatch(regex, "^[a-z0-9-]*$");
    }
}
