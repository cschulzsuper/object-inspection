using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class KebabCaseAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is not string )
            {
                return false;
            }

            return KebabCaseValidator.IsValid((string)value);
        }
    }
}
