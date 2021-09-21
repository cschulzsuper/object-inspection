using System.Text.RegularExpressions;

namespace Super.Paula.Validation
{
    public static class DayNumberValidator
    {
        public static bool IsValid(int dayNumber)
            => dayNumber >= 0;
    }
}
