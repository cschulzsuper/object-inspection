using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

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