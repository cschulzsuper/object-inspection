using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class ExtensionFieldDataTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return ExtensionFieldDataTypeValidator.IsValid(value);
        }
    }
}
