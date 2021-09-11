using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Super.Paula.Shared.Validation
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
