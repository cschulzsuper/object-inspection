using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class ValidStringAttribute : ValidationAttribute
{
    private readonly string[] _range;

    public ValidStringAttribute(params string[] range)
    {
        _range = range;
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is not string stringValue)
        {
            return false;
        }

        return ValidStringValidator.IsValid(stringValue, _range);
    }
}