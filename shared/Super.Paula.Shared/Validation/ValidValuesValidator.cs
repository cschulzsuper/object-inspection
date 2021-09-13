using System.Linq;

namespace Super.Paula.Validation
{
    public static class ValidValuesValidator
    {
        public static bool IsValid(object value, params object[] range)
            => range.Contains(value);
    }
}
