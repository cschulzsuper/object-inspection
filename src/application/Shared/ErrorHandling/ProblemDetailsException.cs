using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Super.Paula.ErrorHandling
{
    public class ProblemDetailsException : HttpRequestException
    {
        public IDictionary<string, string[]>? Errors { get; }

        public ProblemDetailsException(string message, IDictionary<string, string[]>? errors, HttpStatusCode statusCode)
            : base(message, null, statusCode)
        {
            Errors = errors;
        }
    }
}
