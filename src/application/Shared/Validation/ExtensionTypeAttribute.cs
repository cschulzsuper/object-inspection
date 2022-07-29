using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class ExtensionTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return ExtensionTypeValidator.IsValid(value);
        }
    }
}
