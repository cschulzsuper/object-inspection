using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class CronExpressionAttribute : ValidationAttribute
    {

        public bool AllowEmptyStrings = false;

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (AllowEmptyStrings && value is "")
            {
                return true;
            }

            return CronExpressionValidator.IsValid(value);
        }
    }
}
