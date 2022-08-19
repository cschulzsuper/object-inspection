using System;
using System.Linq;

namespace Super.Paula.Validation
{
    public static class InvalidStringValidator
    {
        public static bool IsValid(string value, params string[] range)
            => !range.Contains(value, StringComparer.InvariantCultureIgnoreCase);
    }
}
