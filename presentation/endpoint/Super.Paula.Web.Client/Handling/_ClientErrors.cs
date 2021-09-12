using System.Net;
using System.Text.Json;
using Super.Paula.Web.Shared.Handling.Responses;

namespace Super.Paula.Web.Client.Handling
{
    public static class _ClientErrors
    {
        public static Func<_ProblemDetails,string> ProblemDetailsTitle =
            (problem) => problem.Title;

        public static HttpResponseMessage RuleOutClientError<TProblemDetails>(this HttpResponseMessage responseMessage, Func<TProblemDetails, string> errorMessageFactory)
        {
            if (responseMessage.StatusCode == HttpStatusCode.BadRequest ||
                responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                var responseStream = responseMessage.Content.ReadAsStream();
                var response = JsonSerializer.Deserialize<TProblemDetails>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (response != null)
                {
                    var errorMessage = errorMessageFactory(response);
                    throw new HttpRequestException(errorMessage, null, responseMessage.StatusCode);
                }
            }

            return responseMessage;
        }
    }
}
