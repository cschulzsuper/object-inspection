using System.Text.RegularExpressions;

namespace Super.Paula.Validation
{
    public static partial class LowerCaseValidator
    {
        public static bool IsValid(object value)
            => value is string regex && LowerCaseRegex().IsMatch(regex);

        [RegexGenerator("^[a-z0-9]*$")]
        private static partial Regex LowerCaseRegex();
    }
}
