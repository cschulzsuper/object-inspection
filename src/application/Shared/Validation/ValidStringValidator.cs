using System.Linq;

namespace Super.Paula.Validation
{
    public static class ValidStringValidator
    {
        public static bool IsValid(string value, params string[] range)
            => range.Contains(value);
    }
}
