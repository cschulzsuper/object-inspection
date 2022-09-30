using System.ComponentModel.DataAnnotations;

namespace ChristianSchulz.ObjectInspection.Shared.Validation;

public class EventProcessingStateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        return EventProcessingStateValidator.IsValid(value);
    }
}