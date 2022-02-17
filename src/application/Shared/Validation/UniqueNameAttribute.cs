using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class UniqueNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return UniqueNameValidator.IsValid(value);
        }
    }
}
