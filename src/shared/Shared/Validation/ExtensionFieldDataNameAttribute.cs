using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class ExtensionFieldDataNameAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return ExtensionFieldDataNameValidator.IsValid(value);
    }
}