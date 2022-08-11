using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class InvalidValueAttribute : ValidationAttribute
    {
        private readonly object[] _range;

        public InvalidValueAttribute(params object[] range)
        {
            _range = range;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return InvalidValueValidator.IsValid(value, _range);
        }
    }
}
