using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Super.Paula.Shared.Validation
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
