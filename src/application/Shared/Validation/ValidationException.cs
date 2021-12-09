using System;
using System.Collections.Generic;

namespace Super.Paula.Validation
{
    public class ValidationException : Exception
    {
        public IDictionary<string, FormattableString[]>? Errors { get; }

        public ValidationException(FormattableString message)
            : base(message.ToString())
        {

        }

        public ValidationException(FormattableString message, IDictionary<string, FormattableString[]> errors)
            : base(message.ToString())
        {
            Errors = errors;
        }
    }
}
