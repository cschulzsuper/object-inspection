using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class ExtensionFieldDataNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return ExtensionFieldDataNameValidator.IsValid(value);
        }
    }
}
