using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class DayNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return DayNumberValidator.IsValid(value);
    }
}