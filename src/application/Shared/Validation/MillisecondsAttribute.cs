using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class MillisecondsAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return MillisecondsValidator.IsValid(value);
        }
    }
}
