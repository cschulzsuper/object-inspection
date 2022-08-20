using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

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