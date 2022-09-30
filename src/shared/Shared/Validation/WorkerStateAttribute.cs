using System.ComponentModel.DataAnnotations;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

public class WorkerStateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return WorkerStateValidator.IsValid(value);
    }
}