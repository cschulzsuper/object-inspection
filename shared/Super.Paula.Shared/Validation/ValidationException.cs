using Super.Paula.Shared.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super.Paula.Shared.Validation
{
    public class ValidationException : ErrorException
    {
        public ValidationException(FormattableString message)
            : base(message)
        {

        }
    }
}
