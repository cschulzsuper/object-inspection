using System.Text.RegularExpressions;

namespace Super.Paula.Validation
{
    public static class MillisecondsValidator
    {
        public static bool IsValid(int milliseconds)
            => milliseconds >= 0 && milliseconds < 86400000;
    }
}
