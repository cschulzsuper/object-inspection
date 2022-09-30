using System.ComponentModel.DataAnnotations;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

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