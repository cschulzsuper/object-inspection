using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class ExtensionAggregateTypeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return ExtensionAggregateTypeValidator.IsValid(value);
    }
}