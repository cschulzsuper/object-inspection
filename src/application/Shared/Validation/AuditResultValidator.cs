using System.Linq;

namespace Super.Paula.Validation
{
    public static class AuditResultValidator
    {
        public static bool IsValid(object value)
            => ValidValuesValidator.IsValid(value, string.Empty, "satisfying", "insufficient", "failed");
    }
}
