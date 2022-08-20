using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

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