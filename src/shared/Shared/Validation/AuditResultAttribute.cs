using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Shared.Validation;

public class AuditResultAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return AuditResultValidator.IsValid(value);
    }
}