using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class CamelCaseAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return CamelCaseValidator.IsValid(value);
        }
    }
}
