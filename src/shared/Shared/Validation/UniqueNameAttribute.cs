using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class UniqueNameAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return UniqueNameValidator.IsValid(value);
    }
}