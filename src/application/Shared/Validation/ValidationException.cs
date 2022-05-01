using System;
using System.Collections.Generic;

namespace Super.Paula.Validation
{
    public class ValidationException : Exception, IFormattableException
    {
        public IDictionary<string, FormattableString[]>? Errors { get; }

        public string MessageFormat { get; }

        public object?[] MessageArguments { get; }

        public ValidationException(FormattableString message)
            : base(message.ToString())
        {
            MessageFormat = message.Format;
            MessageArguments = message.GetArguments();
        }

        public ValidationException(FormattableString message, IDictionary<string, FormattableString[]> errors)
            : base(message.ToString())
        {
            Errors = errors;
            MessageFormat = message.Format;
            MessageArguments = message.GetArguments();
        }
    }
}
