using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Super.Paula.ErrorHandling
{
    public class ProblemDetailsException : HttpRequestException
    {
        public IDictionary<string, string[]>? Errors { get; }

        public string Title => Message;

        public string? TitleFormat { get; }

        public object?[]? TitleArguments { get; }

        public ProblemDetailsException(string message, IDictionary<string, string[]>? errors, HttpStatusCode statusCode)
            : base(message, null, statusCode)
        {
            Errors = errors;
        }

        public ProblemDetailsException(string format, object?[] arguments, IDictionary<string, string[]>? errors, HttpStatusCode statusCode)
            : base(string.Format(format, arguments), null, statusCode)
        {
            TitleFormat = format;
            TitleArguments = arguments;
            Errors = errors;
        }

    }
}
