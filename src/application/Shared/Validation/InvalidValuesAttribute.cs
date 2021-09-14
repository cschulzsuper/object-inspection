using System.ComponentModel.DataAnnotations;

namespace Super.Paula.Validation
{
    public class InvalidValuesAttribute : ValidationAttribute
    {
        private readonly object[] _range;

        public InvalidValuesAttribute(params object[] range)
        {
            _range = range;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            return InvalidValuesValidator.IsValid(value, _range);
        }
    }
}
