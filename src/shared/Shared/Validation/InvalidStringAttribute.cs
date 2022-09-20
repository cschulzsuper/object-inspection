using System.ComponentModel.DataAnnotations;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

public class InvalidStringAttribute : ValidationAttribute
{
    private readonly string[] _range;

    public InvalidStringAttribute(params string[] range)
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
            return true;
        }

        return InvalidStringValidator.IsValid(stringValue, _range);
    }
}