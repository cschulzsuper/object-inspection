using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class ValidValuesAttribute : ValidationAttribute
    {
        private readonly object[] _range;

        public ValidValuesAttribute(params object[] range)
        {
            _range = range;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return ValidValuesValidator.IsValid(value, _range);
        }
    }
}
