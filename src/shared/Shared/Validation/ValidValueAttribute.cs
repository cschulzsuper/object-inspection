using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class ValidValueAttribute : ValidationAttribute
{
    private readonly object[] _range;

    public ValidValueAttribute(params object[] range)
    {
        _range = range;
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return ValidValueValidator.IsValid(value, _range);
    }
}