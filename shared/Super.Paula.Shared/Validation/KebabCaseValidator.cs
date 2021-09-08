using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Super.Paula.Shared.Validation
{
    public static class KebabCaseValidator
    {
        public static bool IsValid(string value)
            => Regex.IsMatch((string)value, "^[a-z0-9-]*$");
    }
}
