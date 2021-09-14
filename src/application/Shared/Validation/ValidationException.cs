using System;
using Super.Paula.ErrorHandling;

namespace Super.Paula.Validation
{
    public class ValidationException : ErrorException
    {
        public ValidationException(FormattableString message)
            : base(message)
        {

        }
    }
}
