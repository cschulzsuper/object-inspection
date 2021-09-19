using System;
using System.Collections.Generic;
using Super.Paula.ErrorHandling;

namespace Super.Paula.Validation
{
    public class ValidationException : ErrorException
    {
        public IDictionary<string, FormattableString[]>? Errors { get; }

        public ValidationException(FormattableString message)
            : base(message)
        {

        }

        public ValidationException(FormattableString message, IDictionary<string, FormattableString[]> errors)
            : base(message)
        {
            Errors = errors;
        }
    }
}
