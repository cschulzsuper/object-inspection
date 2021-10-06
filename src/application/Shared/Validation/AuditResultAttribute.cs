using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class AuditResultAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return AuditResultValidator.IsValid(value);
        }
    }
}
