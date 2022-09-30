using System.ComponentModel.DataAnnotations;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

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