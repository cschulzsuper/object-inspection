using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class LowerCaseAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return LowerCaseValidator.IsValid(value);
        }
    }
}
