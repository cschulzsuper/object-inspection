using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
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
}
