using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Super.Paula.Shared.Validation
{
    public static class StringRangeValidator
    {
        public static bool IsValid(string value, params string[] range)
            => range.Contains(value);
    }
}
