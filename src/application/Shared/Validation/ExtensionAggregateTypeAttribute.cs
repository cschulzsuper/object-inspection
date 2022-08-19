using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class ExtensionAggregateTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return ExtensionAggregateTypeValidator.IsValid(value);
        }
    }
}
