using System.Text.RegularExpressions;

namespace Super.Paula.Validation
{
    public static partial class CamelCaseValidator
    {
        public static bool IsValid(object value)
            => value is string regex && CamelCaseRegex().IsMatch(regex);

        [RegexGenerator("^[a-z]+([A-Z0-9][a-z0-9]+)*[A-Za-z0-9]*$")]
        private static partial Regex CamelCaseRegex();
    }
}
