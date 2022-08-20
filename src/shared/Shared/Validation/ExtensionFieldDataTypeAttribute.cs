using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class ExtensionFieldDataTypeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return ExtensionFieldDataTypeValidator.IsValid(value);
    }
}