using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class CamelCaseAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return CamelCaseValidator.IsValid(value);
    }
}