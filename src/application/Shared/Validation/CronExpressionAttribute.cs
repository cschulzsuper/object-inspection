using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class CronExpressionAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value as string == string.Empty)
            {
                return true;
            }

            return CronExpressionValidator.IsValid(value);
        }
    }
}
