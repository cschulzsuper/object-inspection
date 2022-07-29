using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class ExtensionFieldTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return ExtensionFieldTypeValidator.IsValid(value);
        }
    }
}
