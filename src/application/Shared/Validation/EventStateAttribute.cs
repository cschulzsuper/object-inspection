using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class EventStateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return EventStateValidator.IsValid(value);
        }
    }
}
