using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class ContinuationStateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return ContinuationStateValidator.IsValid(value);
        }
    }
}
