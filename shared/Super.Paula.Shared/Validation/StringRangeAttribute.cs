using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Super.Paula.Shared.Validation
{
    public class StringRangeAttribute : ValidationAttribute
    {
        private readonly string[] _range;

        public StringRangeAttribute(params string[] range)
        {
            _range = range;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is not string)
            {
                return false;
            }

            return StringRangeValidator.IsValid((string)value, _range);
        }
    }
}
