using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class DayNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is not int )
            {
                return false;
            }

            return DayNumberValidator.IsValid((int)value);
        }
    }
}
