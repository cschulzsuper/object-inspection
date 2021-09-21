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

            if (value is not int)
            {
                return false;
            }

            return MillisecondsValidator.IsValid((int)value);
        }
    }
}
